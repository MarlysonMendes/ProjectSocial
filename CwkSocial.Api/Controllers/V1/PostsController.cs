using AutoMapper;
using CwkSocial.Api.Contracts.Common;
using CwkSocial.Api.Contracts.Posts.Requests;
using CwkSocial.Api.Contracts.Posts.Responses;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Posts.Command;
using CwkSocial.Application.
    Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _mediator.Send(new GetAllPostsQuery());
            var handleError = new HandlerError();
            var mapped = _mapper.Map<List<PostResponse>>(result.Payload);
            return result.IsError ? handleError.HandleErrorResponse(result.Errors) : Ok(mapped);

        }

        [HttpGet]
        [Route(ApiRoutes.Posts.IdRoute)]
        public async Task<IActionResult> GetById(string id)
        {
            var postId = Guid.Parse(id);
            var query = new GetPostByIdQuery() { PostId = postId };
            var result = await _mediator.Send(query);

            var mapped = _mapper.Map<PostResponse>(result.Payload);
            var handleError = new HandlerError();
            return result.IsError ? handleError.HandleErrorResponse(result.Errors) : Ok(mapped);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreatePost([FromBody] PostCreate newPost)
        {
            var commandPost = new CreatePostCommand()
            {
                UserProfileId = newPost.UserProfileId,
                TextContent = newPost.TextContent
            };

            var result = await _mediator.Send(commandPost);
            
            var handleError = new HandlerError();
            
            var mapped = _mapper.Map<PostResponse>(result.Payload);

            return result.IsError ? handleError.HandleErrorResponse(result.Errors)
                : CreatedAtAction(nameof(GetById), new { id = result.Payload.UserProfileId }, mapped);
        }

        [HttpPatch]
        [Route(ApiRoutes.Posts.IdRoute)]     
        [ValidateModel]
        public async Task<IActionResult> UpdatePostText ([FromBody] PostUpdate postUpdate, string id)
        {
            var commandUpdate = new UpdatePostTextCommand()
            {
                NewText = postUpdate.Text,
                PostId = Guid.Parse(id)
            };
            var result = await _mediator.Send(commandUpdate);

            var handleError = new HandlerError();   

            return result.IsError ? handleError.HandleErrorResponse(result.Errors) : NoContent();

        }
        [HttpDelete]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateModel]
        public async Task<IActionResult> DeletePost (string id)
        {
            var command = new DeletePostCommand { PostId  = Guid.Parse(id)};
            var result = await _mediator.Send(command);


            var handleError = new HandlerError();

            return result.IsError ? handleError.HandleErrorResponse(result.Errors) : NotFound();
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.PostComments)]
        public async Task<IActionResult> GetPostCommentsByPostId(string postId)
        {
            var query = new GetPostCommentsQuery { PostId = Guid.Parse(postId)};
            var result= await _mediator.Send(query);
            
            var handleError = new HandlerError();
            if(result.IsError) handleError.HandleErrorResponse(result.Errors);

            var comments = _mapper.Map<List<PostCommentResponse>>(result.Payload);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.PostComments)]
        public async Task<IActionResult> addCommentToPost(string postId, [FromBody] PostCommentCreate comment)
        {
            var isValidGuid = Guid.TryParse(comment.UserProfileId, out var userProfileId);

            if (!isValidGuid)
            {
                var apiError = new ErrorResponse();


                apiError.StatusCode = 400;
                apiError.StatusPhrase = "Bad request";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add("Provided UserProfile id is not in a valid Guid format");
                return BadRequest(apiError);
            }

            var command = new CreatePostCommentCommand
            { 
                PostId = Guid.Parse(postId), 
                UserProfileId = userProfileId,
                CommentText = comment.Text
            };

            var result = await _mediator.Send(command);

            var handleError = new HandlerError();
            if (result.IsError) return handleError.HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<PostCommentResponse>(result.Payload);
            return Ok(result);
        }
    }
}
