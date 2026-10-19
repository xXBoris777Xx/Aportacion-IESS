using System.Globalization;

namespace Aportacion_IESS
{
    public partial class MainPage : ContentPage
    {
        const decimal IessRate = 0.0945m;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCalcularClicked(object? sender, EventArgs e)
        {
            string nombre = EntryNombre.Text?.Trim() ?? string.Empty;
            string apellido = EntryApellido.Text?.Trim() ?? string.Empty;
            string edadText = EntryEdad.Text?.Trim() ?? string.Empty;
            string salarioText = EntrySalario.Text?.Trim() ?? string.Empty;

            // Validar campos completos
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(edadText) || string.IsNullOrWhiteSpace(salarioText))
            {
                await DisplayAlertAsync("Error", "Todos los campos deben estar completos.", "OK");
                return;
            }

            // Validar edad como número entero
            if (!int.TryParse(edadText, NumberStyles.Integer, CultureInfo.CurrentCulture, out int edad))
            {
                // intentar con InvariantCulture (manejo de separadores)
                if (!int.TryParse(edadText, NumberStyles.Integer, CultureInfo.InvariantCulture, out edad))
                {
                    await DisplayAlertAsync("Error", "Edad debe ser un número entero.", "OK");
                    return;
                }
            }

            // Validar salario como decimal
            // aceptar tanto coma como punto decimal
            string salarioNormalized = salarioText.Replace(',', '.');
            if (!decimal.TryParse(salarioNormalized, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal salario))
                {
                await DisplayAlertAsync("Error", "Salario debe ser un número (usar sólo dígitos y separador decimal).", "OK");
                return;
            }

            // Calcular aporte
            decimal aporte = Math.Round(salario * IessRate, 2);

            // Formatear resultado en moneda según cultura actual
            string aporteFormateado = aporte.ToString("C2", CultureInfo.CurrentCulture);

            string mensaje = $"Bienvenido \"{nombre} {apellido}\"\n" +
                             $"Tienes \"{edad}\" años\n" +
                             $"Tu aporte mensual es \"{aporteFormateado}\"";

            await DisplayAlertAsync("Resultado", mensaje, "OK");
        }
    }
}
