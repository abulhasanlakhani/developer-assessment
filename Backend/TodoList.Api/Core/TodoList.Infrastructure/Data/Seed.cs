using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data;

public static class Seed
{
    public static TodoItem[] TodoList =
    {
        new(new Guid("5660e7b9-7555-4d3f-b863-df658440820b"), "Todo 1", false, new DateTime(2022, 7, 31), -1),
        new(new Guid("cbab58bb-fa24-46b9-b68d-ee25ddefb1a6"), "Todo 2", false, new DateTime(2022, 6, 25), -1),
        new(new Guid("bcb81fd8-ab1d-4874-af23-35513d3d673d"), "Todo 3", false, new DateTime(2022, 5, 15), -1)
    };

    internal static TodoItem[] BlankTodoList = Array.Empty<TodoItem>();
}