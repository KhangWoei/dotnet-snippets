using Crawling.CrawlSource;
using MediatR;

namespace Crawling.Frontier;

internal sealed class SeedRequest : IRequest<ICrawlSource>;