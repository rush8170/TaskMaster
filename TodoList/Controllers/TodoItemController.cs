public class TodoItemController {
    List<TodoItem> todoItems;

    public TodoItemController()
    {  
        todoItems = new List<TodoItem>();    
    }

    public string AddTodoItem(string title, string details, DateTime startTime, DateTime endTime) {
        if(string.IsNullOrEmpty(title)) {
            throw new ArgumentNullException("Title cannot be null");
        }
        if(string.IsNullOrEmpty(details)) {
            throw new ArgumentNullException("Details cannot be null");
        }
        if(startTime < DateTime.Now) {
            throw new ArgumentException("Start time is in the past");
        }
        if(endTime < startTime) {
            throw new ArgumentException("End time cannot be before start time");
        }
        
        var duplicateItem = todoItems.FirstOrDefault(item => {
            return string.Equals(item.Title, title, StringComparison.InvariantCultureIgnoreCase) &&
            startTime >= item.StartTime &&
            startTime <= item.EndTime; 
        });

        if(duplicateItem!=null) {
            return "The item is already present, feel free to update the same if required";
        }

        todoItems.Add(new TodoItem {
            Guid = Guid.NewGuid(),
            Title = title,
            Details = details,
            StartTime = startTime,
            EndTime = endTime    
        });

        return "Successfully added todo items to your list";
    }

    public List<TodoItem> getAllTodoItems() {
        return todoItems;
    }

    public TodoItem? getTodoItem(Guid guid) {
        return todoItems.FirstOrDefault(item => {
            return item.Guid == guid;
        });
    }

    public string deleteTodoItem(Guid guid) {
        var item = todoItems.FirstOrDefault(item => {
            return item.Guid.Equals(guid);
        });
        if(item!=null) {
            todoItems.Remove(item);
            return "Item successfully deleted";
        }
        else {
            return "Item to be deleted is not present";
        }
    }

    public string updateTodoItem(Guid guid, string title, string details, DateTime startTime, DateTime endTime) {
        var item = todoItems.FirstOrDefault(item => {
            return item.Guid.Equals(guid);
        });
        if(item!=null) {
            if(startTime < DateTime.Now) {
            throw new ArgumentException("Start time is in the past");
            }
            if(endTime < startTime) {
                throw new ArgumentException("End time cannot be before start time");
            }
            item.Title = title;
            item.Details = details;
            item.StartTime = startTime;
            item.EndTime = endTime;
            return "Item successfully updated";
        }
        else {
            return "Item to be updated is not present";
        }
    }
}