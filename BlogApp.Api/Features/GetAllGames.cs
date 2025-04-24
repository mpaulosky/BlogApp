// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     GetAllGames.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : BlogApp
// Project Name :  BlogApp.Api
// =======================================================

using Microsoft.AspNetCore.Mvc;
using MyMediator;

namespace BlogApp.Api.Features;

public static class GetAllGames
{
    public record Query : IRequest<List<string>>;

    public class Handler : IRequestHandler<Query, List<string>>
    {
        public Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var games = new List<string> {
                "The Legend of Blazor",
                "Entity Framework Odyssey",
                "Clean Architecture 3D",
                "Vertical Slice: Origins" };
            return Task.FromResult(games);
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class GamesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGames(CancellationToken cancellationToken)
    {
        var games = await sender.Send(new GetAllGames.Query(), cancellationToken);
        return Ok(games);
    }
}
