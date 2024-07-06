I did not use the Authorize attribute because the task did not mention authorization, but ideally, Authorize attributes should be added.
Each endpoint equals one class to make the code grow horizontally rather than vertically - this approach is newer than controllers growing in width.
ApplicationDbContext is used as a repository pattern, hence there are no explicit repositories or services.
