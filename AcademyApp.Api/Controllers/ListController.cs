using AcademyApp.Api.Contracts;
using AcademyApp.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;

        public ListController(IListService listService)
        {
            _listService = listService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetToDoListsByUserId()
        {
            var lists = await _listService.GetToDoListsByUserIdAsync();
            if (lists == null || !lists.Any())
            {
                return NotFound("No to-do lists found for this user.");
            }
            return Ok(lists);
        }

        [HttpGet("{listId}/entries")]
        public async Task<IActionResult> GetToDoListEntriesByListId(int listId)
        {
            var entries = await _listService.GetToDoListEntriesByListIdAsync(listId);
            if (entries == null || !entries.Any())
            {
                return NotFound("No entries found for this to-do list.");
            }
            return Ok(entries);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDoList([FromBody] ToDoList toDoList)
        {
            if (toDoList == null || string.IsNullOrEmpty(toDoList.Title))
            {
                return BadRequest("Invalid to-do list data.");
            }

            await _listService.AddToDoListAsync(toDoList);
            return CreatedAtAction(nameof(GetToDoListsByUserId), new { userId = toDoList.UserId }, toDoList);
        }

        [HttpPost("entry")]
        public async Task<IActionResult> AddToDoListEntry([FromBody] ToDoListEntry toDoListEntry)
        {
            if (toDoListEntry == null || string.IsNullOrEmpty(toDoListEntry.Title))
            {
                return BadRequest("Invalid to-do list entry data.");
            }

            await _listService.AddToDoListEntryAsync(toDoListEntry);
            return CreatedAtAction(nameof(GetToDoListEntriesByListId), new { listId = toDoListEntry.ToDoListId }, toDoListEntry);
        }

        [HttpPut("entry")]
        public async Task<IActionResult> UpdateToDoListEntry([FromBody] ToDoListEntry toDoListEntry)
        {
            if (toDoListEntry == null || toDoListEntry.Id <= 0)
            {
                return BadRequest("Invalid to-do list entry data.");
            }

            await _listService.UpdateToDoListEntryAsync(toDoListEntry);
            return NoContent();
        }

        [HttpDelete("entry/{entryId}")]
        public async Task<IActionResult> DeleteToDoListEntry(int entryId)
        {
            if (entryId <= 0)
            {
                return BadRequest("Invalid entry ID.");
            }

            await _listService.DeleteToDoListEntryAsync(entryId);
            return NoContent();
        }

        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteToDoList(int listId)
        {
            if (listId <= 0)
            {
                return BadRequest("Invalid list ID.");
            }

            await _listService.DeleteToDoListAsync(listId);
            return NoContent();
        }
    }
}
