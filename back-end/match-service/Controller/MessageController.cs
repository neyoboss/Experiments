using Microsoft.AspNetCore.Mvc;

[ApiController]
public class MessageController : ControllerBase
{
    IMessageReceive messageReceive;
    IMatchService matchService;
    public MessageController(IMessageReceive messageReceive, IMatchService matchService)
    {
        this.messageReceive = messageReceive;
        this.matchService = matchService;
    }
}