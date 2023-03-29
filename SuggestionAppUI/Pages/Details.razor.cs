using Microsoft.AspNetCore.Components;

namespace SuggestionAppUI.Pages;

public partial class Details
 {
     [Parameter]
     public string Id { get; set; }

     private SuggestionModel suggestion;
     private UserModel loggedInUser;
     private List<StatusModel> statuses;
     private string settingStatus = "";
     private string urlText = "";
     protected async override Task OnInitializedAsync()
     {
         suggestion = await suggestionData.GetSuggestionById(Id);
         loggedInUser = await provider.GetUserFromAuth(userData);
         statuses = await statusData.GetAllStatuses();
     }

     private void ClosePage()
     {
         navManager.NavigateTo("/");
     }

     private string GetUpvoteTopText()
     {
         if (suggestion.UserVotes?.Count > 0)
         {
             return suggestion.UserVotes.Count.ToString("00");
         }
         else
         {
             if (suggestion.Author.Id == loggedInUser?.Id)
             {
                 return "Awaiting";
             }

             return "Click To";
         }
     }

     private string GetUpvoteBottomText() => suggestion.UserVotes?.Count > 1 ? "Upvotes" : "Upvote";
     private async Task VoteUp()
     {
         if (loggedInUser is not null)
         {
             if (suggestion.Author.Id == loggedInUser.Id)
             {
                 // can't vote on your own suggestion
                 return;
             }

             if (suggestion.UserVotes.Add(loggedInUser.Id) == false)
             {
                 suggestion.UserVotes.Remove(loggedInUser.Id);
             }

             await suggestionData.UpvoteSuggestions(suggestion.Id, loggedInUser.Id);
         }
         else
         {
             navManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
         }
     }

     private string GetVoteClass()
     {
         if (suggestion.UserVotes is null || suggestion.UserVotes.Count == 0)
         {
             return "suggestion-detail-no-votes";
         }
         else if (suggestion.UserVotes is not null && suggestion.UserVotes.Contains(loggedInUser?.Id))
         {
             return "suggestion-detail-voted";
         }
         else
         {
             return "suggestion-detail-not-voted";
         }
     }

     private async Task CompleteSetStatus()
     {
         switch (settingStatus)
         {
             case "completed":
                 if (string.IsNullOrWhiteSpace(urlText))
                 {
                     return;
                 }

                 suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus).First();
                 suggestion.OwnerNotes = $"Hey, hope you didn't think we forgot? We've taken your hot topic and turned it into a resource. You can find it here: <a href='{urlText}' target='blank'>{urlText}</a>";
                 break;
             case "watching":
                 suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus).First();
                 suggestion.OwnerNotes = $"Hi there! We've noticed your suggestion has generated a lot of interest. We're keeping both eyes on it.";
                 break;
             case "upcoming":
                 suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus).First();
                 suggestion.OwnerNotes = $"Nice one! Your suggestion really piqued the interest of our team and other users. We've got something cooking and we'll let you know when its ready!";
                 break;
             case "dismissed":
                 suggestion.SuggestionStatus = statuses.Where(s => s.StatusName.ToLower() == settingStatus).First();
                 suggestion.OwnerNotes = $"Total bummer man but we can't accept your suggestion. Our team has decided its not a good fit for what we offer (sad emoji). We strongly encourage you to try a different suggestion!";
                 break;
             default:
                 return;
         }

         settingStatus = null;
         await suggestionData.UpdateSuggestion(suggestion);
     }

     private string GetStatusClass()
     {
         if (suggestion is null || suggestion.SuggestionStatus is null)
         {
             return "suggestion-detail-status-none";
         }

         string output = suggestion.SuggestionStatus.StatusName switch
         {
             "Completed" => "suggestion-detail-status-completed",
             "Watching" => "suggestion-detail-status-watching",
             "Upcoming" => "suggestion-detail-status-upcoming",
             "Dismissed" => "suggestion-detail-status-dismissed",
             _ => "suggestion-detail-status-none"
         };
         return output;
     }
 }