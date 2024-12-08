using Microsoft.AspNetCore.Components;
using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Components.Chat;
using TeleChat.WebUI.Customs;
using TeleChat.WebUI.Services.App;

namespace TeleChat.WebUI.Components.Reactions
{
    public partial class ReactionPicker
    {
        #region DependencyInjection

        [Inject] public IAppService AppService { get; private set; } = null!;

        #endregion

        #region Properties

        [Parameter] public TCPopover? Popover { get; set; }
        [Parameter] public Message? Message { get; init; }
        [Parameter] public ChatBox? ChatBox { get; init; }

        #endregion

        #region Fields

        private readonly List<Reaction> _reactions = [];

        #endregion

        #region LifecycleEvents

        protected override async Task OnInitializedAsync()
        {
            var result = await AppService.GetReactionsAsync();

            if (result.Count > 0)
            {
                _reactions.AddRange(result);
            }
        }

        #endregion

        #region PrivateMethods

        private async Task AddOrRemoveReactionAsync(Reaction reaction)
        {
            if (Message is null || ChatBox?.GetConnectionId is null || ChatBox?.User is null)
            {
                return;
            }

            var sameUserReaction = ChatBox.Reactions.FirstOrDefault(x => 
                x.UserId == ChatBox.User.Id && 
                x.ReactionId == reaction.Id && 
                x.MessageId == Message.Id);

            try
            {
                if (sameUserReaction is not null)
                {
                    await AppService.RemoveReactionAsync(sameUserReaction);
                }
                else
                {
                    var dto = new ReactionDto()
                    {
                        MessageReactionId = -1,
                        ReactionId = reaction.Id,
                        Value = reaction.Value,
                        UserId = ChatBox.User.Id,
                        MessageId = Message.Id,
                        ConnectionId = ChatBox!.GetConnectionId!
                    };
                    
                    _ = await AppService.AddReactionAsync(dto);
                }
            }
            finally
            {
                Popover?.ClosePopoverMenu();
            }
        }

        #endregion
    }
}