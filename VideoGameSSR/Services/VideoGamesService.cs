using VideoGameSSR.Utility;
using VideoGameSSR.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace VideoGameSSR.Services
{
    public interface IVideoGamesService
    {
        Task <Dictionary<int, VideoGamesModel>> GetAllVideoGames();
        Task <Dictionary<int, VideoGamesModel>> SortedVideoGames(string sorted_by);
        Task <bool> AddVideoGameDetails(VideoGamesModel videoGamesModel);
        Task <bool> UpdateVideoGameDetails(VideoGamesModel videoGamesModel);
        Task <bool> DeleteVideoGameDetails(int videoGameId);
    }

    public class VideoGamesService : IVideoGamesService
    {
        public async Task<Dictionary<int, VideoGamesModel>> GetAllVideoGames()
        {
            string getVideoGamesQuery = "select id, title, publisher, release_year from steam_games";
            Dictionary<int, VideoGamesModel> videoGamesDictionary = new();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(AppSettingsHelper.VideoGamesDbConnection))
                {
                    var videoGames = connection.Query<dynamic>(getVideoGamesQuery).OrderByDescending(game => game.id).ToList();

                    foreach (var game in videoGames)
                    {
                        videoGamesDictionary.Add(game.id, new VideoGamesModel
                        {
                            Id = game.id,
                            Title = game.title,
                            Publisher = game.publisher,
                            ReleaseYear = game.release_year
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return videoGamesDictionary;
        }

        public async Task<Dictionary<int, VideoGamesModel>> SortedVideoGames(string sorted_by)
        {
            Dictionary<int, VideoGamesModel> sortedVideoGamesDictionary = new();
            try
            {
                sortedVideoGamesDictionary = await GetAllVideoGames();
                if (sorted_by == "title")
                    sortedVideoGamesDictionary = sortedVideoGamesDictionary.OrderBy(x => x.Value.Title).ToDictionary(x => x.Key, x => x.Value);
                else if (sorted_by == "publisher")
                    sortedVideoGamesDictionary = sortedVideoGamesDictionary.OrderBy(x => x.Value.Publisher).ToDictionary(x => x.Key, x => x.Value);
                else
                    sortedVideoGamesDictionary = sortedVideoGamesDictionary.OrderBy(x => x.Value.ReleaseYear).ToDictionary(x => x.Key, x => x.Value);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return sortedVideoGamesDictionary;
        }

        public async Task<bool> AddVideoGameDetails(VideoGamesModel videoGamesModel)
        {
            dynamic result = false;
            try
            {
                string addVideoGameQuery = "insert into steam_games (title, publisher, release_year) values (@Title, @Publisher, @ReleaseYear)";
                using (var connection = new MySqlConnection(AppSettingsHelper.VideoGamesDbConnection))
                {
                    var addResult = connection.Execute(addVideoGameQuery, new
                    {
                        videoGamesModel.Title,
                        videoGamesModel.Publisher,
                        videoGamesModel.ReleaseYear
                    });
                    if (addResult > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }

        public async Task<bool> UpdateVideoGameDetails(VideoGamesModel videoGamesModel)
        {
            dynamic result = false;
            try
            {
                string updateVideoGameQuery = "update steam_games set title = @Title, publisher = @Publisher, release_year = @ReleaseYear where id = @Id";
                using (var connection = new MySqlConnection(AppSettingsHelper.VideoGamesDbConnection))
                {
                    var updateResult = connection.Execute(updateVideoGameQuery, new
                    {
                        videoGamesModel.Id,
                        videoGamesModel.Title,
                        videoGamesModel.Publisher,
                        videoGamesModel.ReleaseYear
                    });
                    if (updateResult > 0)
                        return true;
                    else 
                        return false;
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }

        public async Task<bool> DeleteVideoGameDetails(int videoGameId)
        {
            dynamic result = false;
            try
            {
                string deleteVideoGameQuery = "delete from steam_games where id = @Id";
                using (var connection = new MySqlConnection(AppSettingsHelper.VideoGamesDbConnection))
                {
                    var deleteResult = connection.Execute(deleteVideoGameQuery, new
                    {
                        Id = videoGameId
                    });
                    if (deleteResult > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }

    }
}
