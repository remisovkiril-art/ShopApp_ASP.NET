using Microsoft.AspNetCore.Mvc;
using ShopApp.homework2.Models;

namespace ShopApp.homework2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private static List<Item> items = new()
        {
            new Item() { Id = 1, Name = "Pen" },
            new Item() { Id = 2, Name = "Book" },
            new Item() { Id = 3, Name = "Pencil" }
        };

        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetItemById(int id)
        {
            foreach (var item in items)
            {
                if (item.Id == id)
                {
                    return Ok(item);
                }
            }

            return NotFound("Item not found");
        }

        [HttpGet("search")]
        public IActionResult Search(string? name)
        {
            if (name == null || name == "")
            {
                return BadRequest("Name is required");
            }

            List<Item> result = new();

            foreach (var item in items)
            {
                if (item.Name.Contains(name))
                {
                    result.Add(item);
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (item.Name == null || item.Name == "")
            {
                return BadRequest("Name is required");
            }

            int id = 1;

            foreach (var i in items)
            {
                if (i.Id >= id)
                {
                    id = i.Id + 1;
                }
            }

            item.Id = id;

            items.Add(item);

            return CreatedAtAction(
                nameof(GetItemById),
                new { id = item.Id },
                item);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Item newItem)
        {
            foreach (var item in items)
            {
                if (item.Id == id)
                {
                    if (newItem.Name == null || newItem.Name == "")
                    {
                        return BadRequest("Name is required");
                    }

                    item.Name = newItem.Name;

                    return Ok(item);
                }
            }

            return NotFound("Item not found");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Id == id)
                {
                    items.RemoveAt(i);

                    return NoContent();
                }
            }

            return NotFound("Item not found");
        }
    }
}