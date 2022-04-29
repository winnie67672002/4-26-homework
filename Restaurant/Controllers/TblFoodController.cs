#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class TblFoodController : Controller
    {
        private readonly HomeworkContext _context;

        public TblFoodController(HomeworkContext context)
        {
            _context = context;
        }

        // GET: TblFood
        public IActionResult Index()
        {
            return View( _context.TblFoods.ToList());
        }

        // GET: TblFood/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblFood =  _context.TblFoods
                .FirstOrDefault(m => m.Id == id);
            if (tblFood == null)
            {
                return NotFound();
            }

            return View(tblFood);
        }

        // GET: TblFood/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblFood/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Style,Stars,Price,Comment")] TblFood tblFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblFood);
                 _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tblFood);
        }

        // GET: TblFood/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblFood =  _context.TblFoods.Find(id);
            if (tblFood == null)
            {
                return NotFound();
            }
            return View(tblFood);
        }

        // POST: TblFood/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Style,Stars,Price,Comment")] TblFood tblFood)
        {
            if (id != tblFood.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblFood);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblFoodExists(tblFood.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblFood);
        }

        // GET: TblFood/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblFood =  _context.TblFoods
                .FirstOrDefault(m => m.Id == id);
            if (tblFood == null)
            {
                return NotFound();
            }

            return View(tblFood);
        }

        // POST: TblFood/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tblFood =  _context.TblFoods.Find(id);
            _context.TblFoods.Remove(tblFood);
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //Get
        public IActionResult Search()
        {
            ViewData["Message"] = "美食搜尋 GET => 取得表單";
            return View(new TblFoodSearchViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search([Bind("Name,MinStar,MaxStar")] TblFoodSearchParams searchParams)
        {

            var viewModel = new TblFoodSearchViewModel(); // 清出記憶體空間 => 打掃房間
            if (ModelState.IsValid)
            {
                var searchResult = _context.TblFoods.ToList(); // 取出所有資料
                if (!string.IsNullOrEmpty(searchParams.Name)) // 如果有輸入名字，就用名字當條件搜尋
                {
                    searchResult = searchResult
                        .Where(x => x.Name == searchParams.Name)
                        .ToList();
                }

                // 如果攻擊力的範圍合邏輯，就用加攻擊力條件再多篩一次
                if (searchParams.MinStar >= 0
                    && searchParams.MaxStar > 0
                    && searchParams.MinStar < searchParams.MaxStar)
                {
                    // 最小 ATK = 10；最大 ATK = 20
                    // ==> 那些英雄攻擊歷介在 10~20 之間
                    searchResult = searchResult
                        .Where(x => x.Stars >= searchParams.MinStar && x.Stars <= searchParams.MaxStar)
                        .ToList();
                }
                else // 如果攻擊力的範圍不合邏輯，就顯示忽略攻擊力條件字樣
                {
                    ViewData["Message"] = $"（忽略美食餐廳搜尋條件）";
                }

                // 用 ViewData 讓 Controller 與 View 共享資料
                ViewData["Message"] += $"搜尋到 {searchResult.Count} 個好吃餐廳";

                // 把搜尋條件與搜尋結果賦值到 ViewModel 的 property
                // MVC 才有辦法幫我們把資料打包進 Search.cshtml
                viewModel.SearchParams = searchParams;
                viewModel.Foods = searchResult;
            }
            // 回傳 View + ViewModel 進行打包作業
            return View(viewModel);
        }


        private bool TblFoodExists(int id)
        {
            return _context.TblFoods.Any(e => e.Id == id);
        }

    }
}
