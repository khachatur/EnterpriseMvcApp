using EnterpriseMvcApp.Data;
using EnterpriseMvcApp.Models;
using EnterpriseMvcApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace EnterpriseMvcApp.Controllers
{
	[Authorize]
	public class ContentController : Controller
	{
		private readonly IContentService _contentService;
		private readonly ILogger<ContentController> _logger;

		public ContentController(IContentService contentService, ILogger<ContentController> logger)
		{
			_contentService = contentService;
			_logger = logger;
		}

		[Authorize(Roles = "Editor,Admin")]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Editor,Admin")]
		public async Task<IActionResult> Create(Content content)
		{
			if (ModelState.IsValid)
			{
				try
				{
					content.Id = Guid.NewGuid();
					content.CreatedAt = DateTime.UtcNow;
					content.UpdatedAt = DateTime.UtcNow;
					content.AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Set AuthorId to logged-in user's ID

					await _contentService.AddContentAsync(content);
					_logger.LogInformation("Content created successfully with ID: {ContentId}", content.Id);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error creating content");
					ModelState.AddModelError("", "An error occurred while creating the content. Please try again.");
				}
			}
			return View(content);
		}

		[Authorize(Roles = "Editor,Admin")]
		public async Task<IActionResult> Edit(Guid id)
		{
			var content = await _contentService.GetContentByIdAsync(id);
			if (content == null)
			{
				return NotFound();
			}
			return View(content);
		}

		[HttpPost]
		[Authorize(Roles = "Editor,Admin")]
		public async Task<IActionResult> Edit(Content content)
		{
			if (ModelState.IsValid)
			{
				content.UpdatedAt = DateTime.UtcNow;
				content.AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Ensure AuthorId is set correctly

				await _contentService.UpdateContentAsync(content);
				return RedirectToAction(nameof(Index));
			}
			return View(content);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var content = await _contentService.GetContentByIdAsync(id);
			if (content == null)
			{
				return NotFound();
			}
			return View(content);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			await _contentService.DeleteContentAsync(id);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Index()
		{
			var contents = await _contentService.GetAllContentsAsync();
			return View(contents);
		}

		public async Task<IActionResult> Details(Guid id)
		{
			var content = await _contentService.GetContentByIdAsync(id);
			if (content == null)
			{
				return NotFound();
			}
			return View(content);
		}

		public async Task<IActionResult> Versions(Guid contentId)
		{
			var versions = await _contentService.GetContentVersionsAsync(contentId);
			return View(versions);
		}
	}
}
