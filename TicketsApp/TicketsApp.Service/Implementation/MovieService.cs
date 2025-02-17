using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;
using TicketsApp.Repository.Interface;
using TicketsApp.Service.Interface;

namespace TicketsApp.Service.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IUserRepository _userRepository;

        public MovieService(IRepository<Movie> movieRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        public void CreateNewMovie(Movie m)
        {
            m.Tickets = new List<Ticket>();
            _movieRepository.Insert(m);
        }

        public void DeleteMovie(Guid id)
        {
            var movie = _movieRepository.Get(id);
            _movieRepository.Delete(movie);
        }

        public List<Movie> GetAllMovies()
        {
            return _movieRepository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            var movie = _movieRepository.Get(id);
            return movie;
        }

        public void UpdateExistingMovie(Movie m)
        {
            _movieRepository.Update(m);
        }
    }
}
