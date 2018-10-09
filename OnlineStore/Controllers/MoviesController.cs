//using AutoMapper;
//using DAL.Models;
//using DAL.Repositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using OnlineStore.Models.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;

//namespace MvcMovie.Controllers
//{
//    public class MoviesController : Controller
//    {
//        private readonly IMovieRepository _movieRepo;
//        private readonly IMovieGenreRepository _movieGenreRepo;
//        private readonly MapperConfiguration _mapperConfiguration;

//        public MoviesController(IMovieRepository movieRepo, IMovieGenreRepository movieGenreRepo)
//        {
//            _movieRepo = movieRepo;
//            _movieGenreRepo = movieGenreRepo;
//            _mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<Movie, MovieViewModel>();
//                cfg.CreateMap<MovieViewModel, Movie>().AfterMap((src, dest)
//                    => dest.MovieGenre = _movieGenreRepo.Find(x => x.GenreName == src.Genre));
//            });
//        }

//        // GET: /Movies/
//        public ActionResult Index(string movieGenre, string searchString)
//        {
//            List<SelectListItem> movieGenres = _movieGenreRepo.GetGenres().Select(x => new SelectListItem()
//            {
//                Text = x,
//                Value = x
//            }).Prepend(new SelectListItem()
//            {
//                Text = "All",
//                Value = ""
//            }).ToList();

//            var movies = _movieRepo.GetMovies(movieGenre, searchString);

//            //var GenreLst = new List<string>();

//            //var GenreQry = from d in db.Movies
//            //               orderby d.Genre
//            //               select d.Genre;

//            //GenreLst.AddRange(GenreQry.Distinct());
//            //ViewBag.movieGenre = new SelectList(GenreLst);

//            //var movies = from m in db.Movies
//            //             select m;

//            //if (!String.IsNullOrEmpty(searchString))
//            //{
//            //    movies = movies.Where(s => s.Title.Contains(searchString));
//            //}

//            //if (!string.IsNullOrEmpty(movieGenre))
//            //{
//            //    movies = movies.Where(x => x.Genre == movieGenre);
//            //}
//            var viewModel = new MovieIndexViewModel
//            {
//                MovieGenre = movieGenre,
//                SearchString = searchString,
//                MovieGenres = movieGenres,
//                //Movies = _mapperConfiguration.CreateMapper().Map<IList<MovieViewModel>>(movies)
//            };
//            return View(viewModel);
//        }
//        //Example from the training module
//        /*
//MovieDBContext db = new MovieDBContext();
//Movie movie = new Movie();
//movie.Title = "Gone with the Wind";
//db.Movies.Add(movie);
//db.SaveChanges();        // <= Will throw server side validation exception  
//         * */

//        // GET: /Movies/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                return BadRequest();
//            }
//            // Movie movie = db.Movies.Find(id);
//            Movie movie = _movieRepo.Find(x => x.ID == id, x => x.MovieGenre);
//            if (movie == null)
//            {
//                return NotFound();
//            }
//            var vm = _mapperConfiguration.CreateMapper().Map<MovieViewModel>(movie);
//            return View(vm);
//        }

//        // GET: /Movies/Create
//        public ActionResult Create()
//        {
//            ViewBag.Genres = _movieGenreRepo.GetGenres().Select(x => new SelectListItem()
//            {
//                Text = x,
//                Value = x
//            }).Prepend(new SelectListItem()
//            {
//                Text = "All",
//                Value = ""
//            }).ToList();

//            return View(new MovieViewModel
//            {
//                Genre = "Comedy",
//                Price = 3.99M,
//                ReleaseDate = DateTime.Now,
//                Rating = "G",
//                Title = "Ghotst Busters III"
//            });
//        }
//        /*
//public ActionResult Create()
//{
//    return View();
//}

// */
//        // POST: /Movies/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(include: "Title,ReleaseDate,Genre,Price,Rating")] MovieViewModel movieVm)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(movieVm);
//            }
//            var movie = _mapperConfiguration.CreateMapper().Map<Movie>(movieVm);
//            //movie.MovieGenre = _movieGenreRepo.Find(x => x.GenreName == movieVm.Genre);
//            _movieRepo.Add(movie);
//            return RedirectToAction(nameof(Index));
//            //    if (ModelState.IsValid)
//            //{
//            //    db.Movies.Add(movie);
//            //    db.SaveChanges();
//            //    return RedirectToAction("Index");
//            //}

//            //return View(movie);
//        }

//        // GET: /Movies/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                return BadRequest();
//            }

//            ViewBag.Genres = _movieGenreRepo.GetGenres().Select(x => new SelectListItem()
//            {
//                Text = x,
//                Value = x
//            }).Prepend(new SelectListItem()
//            {
//                Text = "All",
//                Value = ""
//            }).ToList();
//            Movie movie = _movieRepo.Find(x => x.ID == id, x => x.MovieGenre);
//            //Movie movie = db.Movies.Find(id);
//            if (movie == null)
//            {
//                return NotFound();
//            }
//            var vm = _mapperConfiguration.CreateMapper().Map<MovieViewModel>(movie);
//            return View(vm);
//        }

//        // POST: /Movies/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(include: "ID,Title,ReleaseDate,Genre,Price,Rating,Timestamp")] MovieViewModel movieVm)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(movieVm);
//            }
//            var movie = _mapperConfiguration.CreateMapper().Map<Movie>(movieVm);
//            _movieRepo.Update(movie);
//            return RedirectToAction(nameof(Index));
//            //if (ModelState.IsValid)
//            //{
//            //    db.Entry(movie).State = EntityState.Modified;
//            //    db.SaveChanges();
//            //    return RedirectToAction("Index");
//            //}
//            //return View(movie);
//        }

//        // GET: /Movies/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return BadRequest();
//                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Movie movie = _movieRepo.Find(id);
//            //Movie movie = db.Movies.Find(id);
//            if (movie == null)
//            {
//                return NotFound();
//            }
//            var vm = _mapperConfiguration.CreateMapper().Map<MovieViewModel>(movie);
//            return View(vm);
//        }

//        // POST: /Movies/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Delete(int id, byte[] timestamp)
//        {
//            _movieRepo.Delete(id, timestamp);
//            //Movie movie = db.Movies.Find(id);
//            //db.Movies.Remove(movie);
//            //db.SaveChanges();
//            return RedirectToAction(nameof(Index));
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                //db.Dispose();
//                _movieRepo.Dispose();
//                _movieGenreRepo.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
