namespace Backend;

// Each node stores one piece of data and TWO links: to the next and the previous.
public class Node<T>
{
    public Node(T data)
    {
        Data = data;
        Next = null;
        Previous = null;
    }

    public T Data { get; set; }
    public Node<T>? Next { get; set; }
    public Node<T>? Previous { get; set; }

}
