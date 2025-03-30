using CollaborativePresentation.Data;
using CollaborativePresentation.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CollaborativePresentation.Hubs
{
    public class PresentationHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public PresentationHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task JoinPresentation(string presentationId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, presentationId);

            var existingConnection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.PresentationId == presentationId && uc.UserName == userName);

            if (existingConnection == null)
            {
                var userConnection = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    PresentationId = presentationId,
                    UserName = userName,
                    IsEditor = false
                };

                _context.UserConnections.Add(userConnection);
                await _context.SaveChangesAsync();
            }
            else
            {
                existingConnection.ConnectionId = Context.ConnectionId;
                await _context.SaveChangesAsync();
            }

            await Clients.Group(presentationId).SendAsync("UserJoined", userName);
            await UpdateUserList(presentationId);
        }

        public async Task LeavePresentation(string presentationId, string userName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, presentationId);

            var userConnection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.PresentationId == presentationId && uc.UserName == userName);

            if (userConnection != null)
            {
                _context.UserConnections.Remove(userConnection);
                await _context.SaveChangesAsync();
            }

            await Clients.Group(presentationId).SendAsync("UserLeft", userName);
            await UpdateUserList(presentationId);
        }

        public async Task ToggleEditor(string presentationId, string userName, bool isEditor)
        {
            var userConnection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.PresentationId == presentationId && uc.UserName == userName);

            if (userConnection != null)
            {
                userConnection.IsEditor = isEditor;
                await _context.SaveChangesAsync();
            }

            await Clients.Group(presentationId).SendAsync("EditorStatusChanged", userName, isEditor);
            await UpdateUserList(presentationId);
        }

        public async Task AddTextElement(string presentationId, string slideId, string content, int x, int y)
        {
            var textElement = new TextElement
            {
                SlideId = slideId,
                Content = content,
                PositionX = x,
                PositionY = y
            };

            _context.TextElements.Add(textElement);
            await _context.SaveChangesAsync();

            await Clients.Group(presentationId).SendAsync("TextElementAdded", slideId, textElement.Id, content, x, y);
        }

        public async Task UpdateTextElement(string presentationId, string elementId, string content)
        {
            var textElement = await _context.TextElements.FindAsync(elementId);
            if (textElement != null)
            {
                textElement.Content = content;
                await _context.SaveChangesAsync();
                await Clients.Group(presentationId).SendAsync("TextElementUpdated", elementId, content);
            }
        }

        public async Task MoveTextElement(string presentationId, string elementId, int x, int y)
        {
            var textElement = await _context.TextElements.FindAsync(elementId);
            if (textElement != null)
            {
                textElement.PositionX = x;
                textElement.PositionY = y;
                await _context.SaveChangesAsync();
                await Clients.Group(presentationId).SendAsync("TextElementMoved", elementId, x, y);
            }
        }

        private async Task UpdateUserList(string presentationId)
        {
            var users = await _context.UserConnections
                .Where(uc => uc.PresentationId == presentationId)
                .ToListAsync();

            await Clients.Group(presentationId).SendAsync("UpdateUserList", users);
        }
    }
}