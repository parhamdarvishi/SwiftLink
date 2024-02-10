﻿using MediatR;
using SwiftLink.Application.Common;
using SwiftLink.Application.Common.Interfaces;

namespace SwiftLink.Application.UseCases.Links.Commands.DisableLink;

public class DisableLinkCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DisableLinkCommand, Result<bool>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<Result<bool>> Handle(DisableLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await _dbContext.Set<Link>().Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        if (link is null)
            return Result.Failure<bool>(LinkMessages.LinkIsNotFound);

        link.IsDisabled = true;
        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken);

        if (dbResult.IsFailure)
            return Result.Failure<bool>(CommonMessages.Database.UpdateFailed);

        return Result.Success(true);
    }
}
