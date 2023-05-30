using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string sourcePath = "";
        string volume = "2.0";  // Volumen por defecto

        if (args.Length < 1)
        {
            Console.Write("Por favor, proporciona la ruta del archivo de origen: ");
            sourcePath = Console.ReadLine();
        }
        else
        {
            sourcePath = args[0];
        }

        // Verifica si el archivo de origen existe
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine("El archivo de origen no existe.");
            return;
        }

        // Si se proporciona un segundo argumento, úsalo para ajustar el volumen
        if (args.Length >= 2)
        {
            volume = args[1];
        }
        else
        {
            Console.Write("Por favor, proporciona el valor de volumen (2.0 es el valor por defecto): ");
            string inputVolume = Console.ReadLine();

            if (!string.IsNullOrEmpty(inputVolume))
            {
                volume = inputVolume;
            }
        }

        string destPath = "";

        // Si se proporciona un tercer argumento, úsalo como la ruta de destino
        if (args.Length >= 3)
        {
            destPath = args[2];
        }
        else // Si no, genera una ruta de destino en un nuevo directorio "SoundBoosted"
        {
            string sourceDirectory = Path.GetDirectoryName(sourcePath);
            string destinationDirectory = Path.Combine(sourceDirectory, "SoundBoosted");

            // Crea el directorio si no existe
            Directory.CreateDirectory(destinationDirectory);

            string sourceFileName = Path.GetFileNameWithoutExtension(sourcePath);
            string sourceExtension = Path.GetExtension(sourcePath);

            // Crea el nombre del archivo de destino con el sufijo
            string destinationFileName = $"{sourceFileName}_sb_{volume}{sourceExtension}";
            destPath = Path.Combine(destinationDirectory, destinationFileName);
        }

        try
        {
            // Define el comando ffmpeg y los parámetros necesarios para aumentar el volumen.
            string arguments = $"-i \"{sourcePath}\" -vcodec copy -scodec copy -filter:a \"volume={volume}\" \"{destPath}\"";

            Process process = new Process();
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Muestra un mensaje de que se esta realizando el aumento de volumen
            Console.WriteLine("Aumentando el volumen...");

            // Leer la salida del proceso
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            process.WaitForExit();

            // Muestra un mensaje cuando el proceso ha finalizado
            Console.WriteLine("El proceso de ffmpeg ha finalizado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al ejecutar ffmpeg: {ex.Message}");
        }
    }
}
