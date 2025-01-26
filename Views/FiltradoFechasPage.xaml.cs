using System.Collections.ObjectModel;

namespace MiniCore.Views;

public partial class FiltradoFechasPage : ContentPage
{
    public ObservableCollection<ResultadoFiltrado> Resultados { get; set; } = new ObservableCollection<ResultadoFiltrado>();

    public FiltradoFechasPage()
    {
        InitializeComponent();
        ResultadosCollectionView.ItemsSource = Resultados;
    }

    private async void OnFiltrarClicked(object sender, EventArgs e)
    {
        // Validar que la fecha inicial no sea mayor a la fecha final
        if (FechaInicioPicker.Date > FechaFinPicker.Date)
        {
            await DisplayAlert("Error", "La fecha inicial no puede ser mayor que la fecha final.", "OK");
            return;
        }

        try
        {
            // Limpiar resultados anteriores
            Resultados.Clear();

            using (var context = new AppDbContext())
            {
                var fechaInicio = FechaInicioPicker.Date;
                var fechaFin = FechaFinPicker.Date;

                // Consulta LINQ para filtrar y agrupar por departamento
                var resultados = context.Gastos
                    .Where(g => g.Fecha >= fechaInicio && g.Fecha <= fechaFin)
                    .GroupBy(g => g.Departamento.Nombre)
                    .Select(grupo => new ResultadoFiltrado
                    {
                        Departamento = grupo.Key,
                        Total = grupo.Sum(g => g.Monto)
                    })
                    .ToList();

                // Agregar resultados al CollectionView
                foreach (var resultado in resultados)
                {
                    Resultados.Add(resultado);
                }
            }
        }
        catch (Exception ex)
        {
            // Mostrar mensaje de error en caso de excepción
            await DisplayAlert("Error", $"Ocurrió un error al filtrar los datos: {ex.Message}", "OK");
        }
    }


    public class ResultadoFiltrado
    {
        public string Departamento { get; set; }
        public decimal Total { get; set; }
    }
    private async void OnEmpleadoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmpleadoPage());
    }
    private async void OnGastosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GastoPage());
    }
    private async void OnDepartamentoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DepartamentoPage());
    }
}
