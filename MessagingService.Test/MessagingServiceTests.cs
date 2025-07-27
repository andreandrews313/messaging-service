using System.Text;
using MessagingService.Controllers;
using MessagingService.Interfaces;
using MessagingService.Models;
using MessagingService.Providers;
using MessagingService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;

namespace MessagingService.Test;

public class MessagingServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly MessageService _messageService;

    public MessagingServiceTests()
    {
        // Setup in-memory DB
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new AppDbContext(options);

        var smsLogger = NullLogger<MockSmsProvider>.Instance;
        var emailLogger = NullLogger<MockEmailProvider>.Instance;

        // Register providers manually
        var mockSms = new MockSmsProvider(smsLogger);
        var mockEmail = new MockEmailProvider(emailLogger);

        var factory = new MessageProviderFactory(new Dictionary<string, IMessageProvider>
        {
            { "sms", mockSms },
            { "mms", mockSms },
            { "email", mockEmail }
        });

        _messageService = new MessageService(_dbContext, factory);
    }

    [Fact]
    public async Task SendMessageAsync_CreatesMessageAndConversation()
    {
        // Arrange
        var message = new OutboundSmsMms
        {
            From = "+1234567890",
            To = "+1987654321",
            Type = "sms",
            Body = "Hello!",
            Timestamp = DateTime.UtcNow,
            Attachments = new List<string>()
        };       

        // Act
        var result = await _messageService.SendMessageAsync(message);

        // Assert
        var messages = await _dbContext.Messages.ToListAsync();
        var conversations = await _dbContext.Conversations.ToListAsync();

        Assert.Single(messages);
        Assert.Single(conversations);
        Assert.Equal("Hello!", messages[0].Body);
        Assert.Equal(messages[0].ConversationId, conversations[0].Id);
    }
}
