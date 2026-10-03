using Microsoft.AspNetCore.Http;
using NooshApp.Api.Dtos;
using NooshApp.Api.Helpers;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    public class CareersService : ICareersService
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly ISupportingDocumentRepository _supportingDocumentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        private static readonly string[] ScreeningKeywords = new[]
        {
            "customer service", "restaurant", "kitchen", "management",
            "cooking", "hospitality", "cashier", "pos", "food safety", "communication"
        };
        private const int ShortlistThreshold = 3;
        private static readonly string[] AllowedDocExtensions = { ".pdf", ".docx", ".jpg", ".jpeg", ".png" };
        private const int MaxSupportingDocs = 3;
        private const long MaxDocSizeBytes = 5 * 1024 * 1024;

        public CareersService(
            IJobApplicationRepository applicationRepository,
            ISupportingDocumentRepository supportingDocumentRepository,
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _applicationRepository = applicationRepository;
            _supportingDocumentRepository = supportingDocumentRepository;
            _environment = environment;
            _emailService = emailService;
        }

        public async Task<JobApplicationDto> SubmitApplicationAsync(
            string fullName, string phoneNumber, string email, string desiredPosition,
            string? coverLetter, IFormFile cvFile, List<IFormFile>? supportingDocuments)
        {
            var savedPath = await SaveFileAsync(cvFile, "cvs");
            var cvText = CvTextExtractor.ExtractText(savedPath);
            var score = ScoreCvText(cvText);

            var application = new JobApplication
            {
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Email = email,
                DesiredPosition = desiredPosition,
                CoverLetter = coverLetter,
                CvFilePath = GetRelativePath(savedPath),
                CvOriginalFileName = cvFile.FileName,
                KeywordScore = score,
                Status = score >= ShortlistThreshold ? ApplicationStatus.Shortlisted : ApplicationStatus.Rejected,
                SubmittedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);

            var savedDocPaths = new List<string>();

            if (supportingDocuments != null && supportingDocuments.Any())
            {
                var validDocs = supportingDocuments
                    .Where(f => f != null && f.Length > 0)
                    .Take(MaxSupportingDocs) // silently caps rather than rejecting the whole submission
                    .ToList();

                var docsToSave = new List<SupportingDocument>();

                foreach (var doc in validDocs)
                {
                    var extension = Path.GetExtension(doc.FileName).ToLowerInvariant();
                    if (!AllowedDocExtensions.Contains(extension)) continue; // skip invalid ones quietly
                    if (doc.Length > MaxDocSizeBytes) continue;

                    var docSavedPath = await SaveFileAsync(doc, "supporting");
                    savedDocPaths.Add(docSavedPath);

                    docsToSave.Add(new SupportingDocument
                    {
                        JobApplicationId = application.Id,
                        FilePath = GetRelativePath(docSavedPath),
                        OriginalFileName = doc.FileName
                    });
                }

                if (docsToSave.Any())
                {
                    await _supportingDocumentRepository.AddRangeAsync(docsToSave);
                }
            }

            var allAttachmentPaths = new List<string> { savedPath };
            allAttachmentPaths.AddRange(savedDocPaths);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendCareerApplicationNotificationAsync(application, allAttachmentPaths);
                }
                catch
                {
                    // EmailService logs internally — this is a final safety net.
                }
            });

            return new JobApplicationDto
            {
                Id = application.Id,
                FullName = application.FullName,
                DesiredPosition = application.DesiredPosition,
                Status = application.Status.ToString(),
                SubmittedAt = application.SubmittedAt
            };
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", subfolder);
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);
            return fullPath;
        }

        private string GetRelativePath(string fullPath)
        {
            var webRoot = _environment.WebRootPath ?? "wwwroot";
            var relative = Path.GetRelativePath(webRoot, fullPath);
            return "/" + relative.Replace("\\", "/");
        }

        private int ScoreCvText(string cvText)
        {
            if (string.IsNullOrWhiteSpace(cvText)) return 0;
            var lowerText = cvText.ToLowerInvariant();
            return ScreeningKeywords.Count(keyword => lowerText.Contains(keyword));
        }
    }
}