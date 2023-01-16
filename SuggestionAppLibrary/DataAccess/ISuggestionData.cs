namespace SuggestionAppLibrary.DataAccess;

public interface ISuggestionData
{
   Task CreateSuggestion(SuggestionModel suggestion);
   Task<List<SuggestionModel>> GetAllApprovedSuggestions();
   Task<List<SuggestionModel>> GetAllSuggestions();
   Task<List<SuggestionModel>> GetAllSuggestionsAwaitingApproval();
   Task<SuggestionModel> GetSuggestionById(string id);
   Task<List<SuggestionModel>> GetUserSuggestions(string userId);
   Task UpdateSuggestion(SuggestionModel suggestion);
   Task UpvoteSuggestions(string suggestionId, string userId);
}