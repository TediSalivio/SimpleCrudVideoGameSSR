using Microsoft.AspNetCore.Components;
using VideoGameSSR.Models;
using VideoGameSSR.Services;

namespace VideoGameSSR.Components.Pages
{
    public partial class VideoGames : IDisposable
    {
        #region INJECTIONS
        [Inject] VideoGamesService? _videoGamesService { get; set; }
        #endregion

        #region MODEL / DICTIONARY / LIST

        [SupplyParameterFromForm (FormName = "AddVideoGameForm")]
        private VideoGamesModel addVideoGameModel { get; set; } = new();

        [SupplyParameterFromForm (FormName = "EditVideoGameForm")]
        private VideoGamesModel editVideoGameModel { get; set; } = new();

        [SupplyParameterFromForm (FormName = "DeleteVideoGameForm")]
        private VideoGamesModel deleteVideoGameModel { get; set; } = new();

        private Dictionary<int, VideoGamesModel> videoGamesDictionary = new();
        #endregion

        #region FIELDS
        [SupplyParameterFromForm (FormName = "SearchVideoGameForm")]
        private string? searchVideoGame { get; set; }

        [SupplyParameterFromForm (FormName = "SortVideoGameForm")]
        private string? sortVideoGame { get; set; }

        private bool isLoading;
        private string? message;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadVideoGamesList();
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void Dispose()
        {
            videoGamesDictionary.Clear();
        }

        private async Task LoadVideoGamesList()
        {
            isLoading = true;
            await Task.Delay(1000);
            videoGamesDictionary = await _videoGamesService.GetAllVideoGames();
            isLoading = false;
        }

        private async Task SearchNameByTitle()
        {
            try
            {
                var filteredGame = videoGamesDictionary
                                   .Where(x => x.Value.Title.Contains(searchVideoGame, StringComparison.OrdinalIgnoreCase) ||
                                          x.Value.Publisher.Contains(searchVideoGame, StringComparison.OrdinalIgnoreCase) ||
                                          x.Value.ReleaseYear.ToString().Contains(searchVideoGame, StringComparison.OrdinalIgnoreCase))
                                   .ToDictionary(x => x.Key, x => x.Value);

                if (!string.IsNullOrWhiteSpace(searchVideoGame))
                {
                    if (filteredGame.Count() > 0)
                    {
                        videoGamesDictionary = filteredGame;
                    }
                    else
                    {
                        videoGamesDictionary.Clear();
                        message = "Searched game cannot be found";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task SortVideoGame()
        {
            try
            {
                videoGamesDictionary = await _videoGamesService.SortedVideoGames(sortVideoGame);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task AddGame()
        {
            var addVideoGameResult = await _videoGamesService.AddVideoGameDetails(addVideoGameModel);
            if (addVideoGameResult)
            {
                message = "Added game successfully";
            }
            else
            {
                message = "Adding game failed";
            }
            videoGamesDictionary = await _videoGamesService.GetAllVideoGames();
            addVideoGameModel = new();
        }

        private async Task UpdateGame()
        {
            var updateVideoGameResult = await _videoGamesService.UpdateVideoGameDetails(editVideoGameModel);
            if(updateVideoGameResult)
            {
                message = "Updated game successfully";
            }
            else
            {
                message = "Updating game failed";
            }
            videoGamesDictionary = await _videoGamesService.GetAllVideoGames();
        }

        private async Task DeleteGame()
        {
            var deleteVideoGameResult = await _videoGamesService.DeleteVideoGameDetails(deleteVideoGameModel.Id);
            if (deleteVideoGameResult)
            {
                message = "Deleted game successfully";
            }
            else
            {
                message = "Deleting game failed";
            }
            videoGamesDictionary = await _videoGamesService.GetAllVideoGames();
        }
    }

}
