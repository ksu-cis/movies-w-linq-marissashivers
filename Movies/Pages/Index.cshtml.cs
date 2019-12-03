using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        //public List<Movie> Movies;
        public IEnumerable<Movie> Movies;

        [BindProperty]
        public string search { get; set; }

        [BindProperty]
        public List<string> mpaa { get; set; } = new List<string>();

        [BindProperty]
        public float? minIMDB { get; set; }

        [BindProperty]
        public float? maxIMDB { get; set; }

        // sort order
        [BindProperty]
        public string sort { get; set; }


        public void OnGet()
        {
            //Movies = MovieDatabase.All;

            // order by title
            Movies = MovieDatabase.All.OrderBy(movie => movie.Title);
        }

        public void OnPost()
        {
            Movies = MovieDatabase.All;

            if (search != null)
            {
                //Movies = MovieDatabase.Search(Movies, search);
                Movies = Movies.Where(movie => movie.Title.Contains(search, StringComparison.OrdinalIgnoreCase) 
                                         || movie.Director != null && movie.Director.Contains(search, StringComparison.OrdinalIgnoreCase)
                );
            }

            if(mpaa.Count != 0)
            {
                //Movies = MovieDatabase.FilterByMPAA(Movies, mpaa);
                Movies = Movies.Where(movie => mpaa.Contains(movie.MPAA_Rating));
            }

            if(minIMDB != null)
            {
                //Movies = MovieDatabase.FilterByMinIMDB(Movies, (float)minIMDB);
                Movies = Movies.Where(movie => movie.IMDB_Rating != null && movie.IMDB_Rating >= minIMDB);
            }

            if (maxIMDB != null)
            {
                Movies = Movies
                    .Where(movie => movie.IMDB_Rating != null)
                    .Where(movie => movie.IMDB_Rating <= maxIMDB);
            }

            //sort option
            if (sort != null)
            {
                // pull out key based on sort selected -- switch statement
                switch(sort)
                {
                    case "title":
                        Movies = Movies.OrderBy(movie => movie.Title);
                        break;
                    case "director":
                        Movies = Movies.OrderBy(movie => movie.Director);
                        break;
                    case "year":
                        Movies = Movies.OrderBy(movie => movie.Release_Year);
                        break;
                    case "imdb":
                        Movies = Movies.OrderBy(movie => movie.IMDB_Rating);
                        break;
                    case "rt":
                        Movies = Movies.OrderBy(movie => movie.Rotten_Tomatoes_Rating);
                        break;
                } // end switch
            }
            
        }
    }
}
