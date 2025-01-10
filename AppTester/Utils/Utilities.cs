using System.Collections.ObjectModel;
using System.Linq;

namespace AppTester.Utils
{
    public enum IOType
    {
        Input = 0,
        Output = 1
    }

    public static class Utilities
    {
        public static IOType GetIOTypeFromCommandParameter(object commandParameter)
        {
            if (commandParameter is not string param)
                throw new ArgumentException("Invalid CommandParameter value.");

            string normalizedParam = param.ToLowerInvariant();

            return normalizedParam switch
            {
                "input" => IOType.Input,
                "output" => IOType.Output,
                _ => throw new ArgumentException("Invalid CommandParameter value.")
            };
        }

        public static void MoveUpElementInAList<T>(ObservableCollection<T> list, T element)
        {
            int indexOfItem = list.IndexOf(element);
            if (indexOfItem == -1)
                throw new ArgumentException($"{element} does not exist in the list.");

            // If the element is already at the top of the list, no need to move
            if (indexOfItem == 0)
                return;

            // Move the element up by swapping with the previous element
            T itemToMove = list[indexOfItem];
            list.RemoveAt(indexOfItem);
            list.Insert(indexOfItem - 1, itemToMove);
        }
        public static void MoveDownElementInAList<T>(ObservableCollection<T> list, T element)
        {
            int indexOfItem = list.IndexOf(element);
            if (indexOfItem == -1)
                throw new ArgumentException($"{element} does not exist in the list.");

            // If the element is already at the bottom of the list, no need to move
            if (indexOfItem == list.Count-1)
                return;

            // Move the element down by swapping with the next element
            T itemToMove = list[indexOfItem];
            list.RemoveAt(indexOfItem);
            list.Insert(indexOfItem + 1, itemToMove);
        }

    }
}
