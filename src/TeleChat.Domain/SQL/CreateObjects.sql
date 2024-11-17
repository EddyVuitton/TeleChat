create or alter procedure [dbo].[p_GetChatReactions] @chatId int
as
begin
	select
		mr.id [MessageReactionId]
		,r.Id [ReactionId]
		,r.Value
		,mr.UserId
		,MessageId
	from GroupChat gc
	join Message m on gc.Id = m.GroupChatId
	join MessageReaction mr on mr.MessageId = m.Id
	join Reaction r on r.Id = mr.ReactionId
	where gc.Id = @chatId
end