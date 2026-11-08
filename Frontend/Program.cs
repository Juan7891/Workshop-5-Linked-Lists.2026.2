using Backend;
using System.Globalization;

// The list is generic: the user chooses which type of data to work with
Console.WriteLine("¿Con qué tipo de dato desea trabajar?");
Console.WriteLine("1. Números enteros (int).");
Console.WriteLine("2. Números decimales (double).");
Console.WriteLine("3. Texto (string).");
Console.Write("Elija una opción: ");
var type = Console.ReadLine();

switch (type)
{
    case "2":
        Run<double>(text => double.Parse(text, CultureInfo.InvariantCulture));
        break;
    case "3":
        Run<string>(text => text);
        break;
    default:
        Run<int>(int.Parse);
        break;
}

// Main loop of the menu. "parse" converts the text written by the user to the chosen type T (int.Parse, double.Parse, or the text itself).
void Run<T>(Func<string, T> parse) where T : IComparable<T>
{
    var list = new DoublyLinkedList<T>();
    var option = string.Empty;

    do
    {
        option = Menu();
        Console.WriteLine();
        switch (option)
        {
            case "1":
                AddValues(list, parse);
                break;

            case "2":
                Console.WriteLine($"Hacia adelante: {list.GetForward()}");
                break;

            case "3":
                Console.WriteLine($"Hacia atrás: {list.GetBackward()}");
                break;

            case "4":
                if (list.SortDescending())
                    Console.WriteLine($"Lista ordenada descendentemente: {list.GetForward()}");
                else
                    Console.WriteLine("La lista ya está ordenada descendentemente.");
                break;

            case "5":
                ShowModes(list.GetModes());
                break;

            case "6":
                Console.WriteLine(list.GetGraph());
                break;

            case "7":
                if (TryRead("Ingrese el valor a buscar: ", parse, out var search))
                {
                    Console.WriteLine(list.Contains(search)
                        ? $"Sí existe el {search} en la lista."
                        : $"No existe el {search} en la lista.");
                }
                break;

            case "8":
                if (TryRead("Ingrese el valor a eliminar (una ocurrencia): ", parse, out var toRemove))
                {
                    Console.WriteLine(list.RemoveFirst(toRemove)
                        ? $"Se eliminó una ocurrencia del {toRemove}. Lista: {list.GetForward()}"
                        : $"No existe el {toRemove} en la lista.");
                }
                break;

            case "9":
                if (TryRead("Ingrese el valor a eliminar (todas las ocurrencias): ", parse, out var toRemoveAll))
                {
                    var removed = list.RemoveAll(toRemoveAll);
                    Console.WriteLine(removed > 0
                        ? $"Se eliminaron {removed} ocurrencia(s) del {toRemoveAll}. Lista: {list.GetForward()}"
                        : $"No existe el {toRemoveAll} en la lista.");
                }
                break;

            case "0":
                Console.WriteLine("¡Hasta luego!");
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
        Console.WriteLine();
    } while (option != "0");
}

// Add one or more values written on a single line, separated by commas. Each value is inserted in its sorted position; invalid ones are reported and skipped.
void AddValues<T>(DoublyLinkedList<T> list, Func<string, T> parse) where T : IComparable<T>
{
    Console.Write("Ingrese los valores separados por coma: ");
    var values = Console.ReadLine()!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (values.Length == 0)
    {
        Console.WriteLine("No se ingresaron valores.");
        return;
    }

    foreach (var text in values)
    {
        try
        {
            list.Add(parse(text));
        }
        catch (FormatException)
        {
            Console.WriteLine($"'{text}' no es válido para el tipo de dato seleccionado y no se adicionó.");
        }
    }
    Console.WriteLine($"Lista: {list.GetForward()}");
}

// Reads a value and converts it to type T. Returns false if the format is invalid
bool TryRead<T>(string message, Func<string, T> parse, out T value)
{
    Console.Write(message);
    var input = Console.ReadLine()!;
    try
    {
        value = parse(input);
        return true;
    }
    catch (FormatException)
    {
        Console.WriteLine("Valor inválido para el tipo de dato seleccionado.");
        value = default!;
        return false;
    }
}

// Show "Fashion is X." or "Fashions are X, Y, and Z."
void ShowModes<T>(List<T> modes)
{
    if (modes.Count == 0)
    {
        Console.WriteLine("La lista está vacía, no hay moda.");
    }
    else if (modes.Count == 1)
    {
        Console.WriteLine($"La moda es {modes[0]}.");
    }
    else
    {
        var allButLast = string.Join(", ", modes.Take(modes.Count - 1));
        Console.WriteLine($"Las modas son {allButLast} y {modes[^1]}.");
    }
}

string Menu()
{
    Console.WriteLine("1. Adicionar.");
    Console.WriteLine("2. Mostrar hacia adelante.");
    Console.WriteLine("3. Mostrar hacia atrás.");
    Console.WriteLine("4. Ordenar descendentemente.");
    Console.WriteLine("5. Mostrar la(s) moda(s).");
    Console.WriteLine("6. Mostrar gráfico.");
    Console.WriteLine("7. Existe.");
    Console.WriteLine("8. Eliminar una ocurrencia.");
    Console.WriteLine("9. Eliminar todas las ocurrencias.");
    Console.WriteLine("0. Salir.");
    Console.Write("Elija una opción: ");
    return Console.ReadLine()!;
}