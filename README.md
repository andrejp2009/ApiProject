## Project Notes

### Authorization
I did not use the `Authorize` attribute because the task did not mention authorization. However, ideally, `Authorize` attributes should be added to secure the endpoints.

### Endpoint Structure
Each endpoint is defined in a separate class to make the code grow horizontally rather than vertically. This approach is more modern compared to controllers growing in width.

### Repository Pattern
`ApplicationDbContext` is used as a repository pattern. Therefore, there are no explicit repositories or services in the code.
