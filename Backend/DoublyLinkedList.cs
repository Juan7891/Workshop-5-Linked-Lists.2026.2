namespace Backend;

// Generic doubly linked list that ALWAYS stays sorted.

public class DoublyLinkedList<T> where T : IComparable<T>
{
    private Node<T>? _head;          // first Node
    private Node<T>? _tail;          // last Node
    private bool _isAscending;       // true = ascending, false = descendant

    public DoublyLinkedList()
    {
        _head = null;
        _tail = null;
        _isAscending = true;
    }

    public int Count { get; private set; }
    public bool IsEmpty => _head == null;

    // ADD: inserts the data directly in its sorted position
    public void Add(T data)
    {
        var newNode = new Node<T>(data);

        if (_head == null)
        {
            _head = newNode;
            _tail = newNode;
            Count++;
            return;
        }

        var current = _head;
        while (current != null && !GoesBefore(data, current.Data))
        {
            current = current.Next;
        }

        if (current == null)
        {
            newNode.Previous = _tail;
            _tail!.Next = newNode;
            _tail = newNode;
        }
        else if (current == _head)
        {
            newNode.Next = _head;
            _head.Previous = newNode;
            _head = newNode;
        }
        else
        {
            newNode.Previous = current.Previous;
            newNode.Next = current;
            current.Previous!.Next = newNode;
            current.Previous = newNode;
        }
        Count++;
    }

    // The < (strict) is used so that duplicates end up AFTER their equals.
    private bool GoesBefore(T newData, T existingData)
    {
        var comparison = newData.CompareTo(existingData);
        return _isAscending ? comparison < 0 : comparison > 0;
    }

    // SHOW FORWARD: traverse from _head to _tail using Next
    public string GetForward()
    {
        if (IsEmpty) return "La lista está vacía.";

        var output = "null <- ";
        var current = _head;
        while (current != null)
        {
            output += current.Next == null ? $"{current.Data}" : $"{current.Data} <-> ";
            current = current.Next;
        }
        return $"{output} -> null";
    }

    // SHOW BACKWARDS: go from _tail to _head using Previous
    public string GetBackward()
    {
        if (IsEmpty) return "La lista está vacía.";

        var output = "null <- ";
        var current = _tail;
        while (current != null)
        {
            output += current.Previous == null ? $"{current.Data}" : $"{current.Data} <-> ";
            current = current.Previous;
        }
        return $"{output} -> null";
    }

    // SORT DESCENDING: it changes the actual structure of the list.
    public bool SortDescending()
    {
        if (!_isAscending) return false;   // was already descending

        var current = _head;
        while (current != null)
        {
            var temp = current.Next;            // I save the following original
            current.Next = current.Previous;    // I exchange the links
            current.Previous = temp;
            current = temp;                     // I move on to the next original
        }

        (_head, _tail) = (_tail, _head);        // the tail becomes the head
        _isAscending = false;                   // the new insertions respect the descending order
        return true;
    }

    // Support method for 5 and 6: count the occurrences of each value.
    private List<(T Value, int Count)> GetOccurrences()
    {
        var occurrences = new List<(T Value, int Count)>();
        var current = _head;
        while (current != null)
        {
            var value = current.Data;
            var count = 0;
            while (current != null && current.Data.CompareTo(value) == 0)
            {
                count++;
                current = current.Next;
            }
            occurrences.Add((value, count));
        }
        return occurrences;
    }

    // MODE(S): the value(s) with the highest number of occurrences
    public List<T> GetModes()
    {
        var modes = new List<T>();
        var occurrences = GetOccurrences();

        var maxCount = 0;
        foreach (var item in occurrences)
        {
            if (item.Count > maxCount) maxCount = item.Count;
        }

        foreach (var item in occurrences)
        {
            if (item.Count == maxCount) modes.Add(item.Value);
        }
        return modes;
    }

    // GRAPH: an asterisk for each occurrence
    public string GetGraph()
    {
        if (IsEmpty) return "La lista está vacía.";

        var output = string.Empty;
        foreach (var item in GetOccurrences())
        {
            output += $"{item.Value} {new string('*', item.Count)}\n";
        }
        return output.TrimEnd('\n');
    }

    // EXISTS: is there at least one node with that value?
    public bool Contains(T data)
    {
        return Find(data) != null;
    }

    // Returns the first node with that value, or null if it doesn't exist
    private Node<T>? Find(T data)
    {
        var current = _head;
        while (current != null)
        {
            if (current.Data.CompareTo(data) == 0) return current;
            current = current.Next;
        }
        return null;
    }

    // DELETE AN OCCURRENCE: deletes the first one found
    public bool RemoveFirst(T data)
    {
        var node = Find(data);
        if (node == null) return false;

        Unlink(node);
        return true;
    }

    // REMOVE ALL OCCURRENCES: returns how many it deleted
    public int RemoveAll(T data)
    {
        var removed = 0;
        var current = _head;
        while (current != null)
        {
            var next = current.Next;           
            if (current.Data.CompareTo(data) == 0)
            {
                Unlink(current);
                removed++;
            }
            current = next;
        }
        return removed;
    }

    // Disconnect a node by reconnecting its neighbors to each other
    private void Unlink(Node<T> node)
    {
        if (node.Previous == null) _head = node.Next;          // it was the head
        else node.Previous.Next = node.Next;

        if (node.Next == null) _tail = node.Previous;          // it was the tail
        else node.Next.Previous = node.Previous;

        node.Next = null;
        node.Previous = null;
        Count--;
    }

    public override string ToString() => GetForward();

}
