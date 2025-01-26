using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniCore.Models;
using System.Collections.ObjectModel;

namespace MiniCore.Views;

public partial class GastoPage : ContentPage
{
    public ObservableCollection<Gasto> Gastos { get; set; } = new ObservableCollection<Gasto>();
    private Gasto _selectedGasto;

    public GastoPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            using (var context = new AppDbContext())
            {
                // Cargar lista de gastos
                var gastos = context.Gastos.ToList();
                Gastos.Clear();
                foreach (var gasto in gastos)
                {
                    Gastos.Add(gasto);
                }

                // Cargar lista de empleados
                EmpleadoPicker.ItemsSource = context.Empleados.ToList();

                // Cargar lista de departamentos
                DepartamentoPicker.ItemsSource = context.Departamentos.ToList();
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Ocurrió un error al cargar los datos: {ex.Message}", "OK");
        }
    }


    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(DescripcionEntry.Text) &&
            decimal.TryParse(MontoEntry.Text, out var monto) &&
            EmpleadoPicker.SelectedItem is Empleado empleado &&
            DepartamentoPicker.SelectedItem is Departamento departamento)
        {
            using (var context = new AppDbContext())
            {
                if (_selectedGasto == null)
                {
                    // Crear un nuevo gasto
                    var nuevoGasto = new Gasto
                    {
                        Descripcion = DescripcionEntry.Text,
                        Monto = monto,
                        Fecha = FechaPicker.Date,
                        EmpleadoId = empleado.Id,
                        DepartamentoId = departamento.Id
                    };
                    context.Gastos.Add(nuevoGasto);
                }
                else
                {
                    // Actualizar gasto existente
                    _selectedGasto.Descripcion = DescripcionEntry.Text;
                    _selectedGasto.Monto = monto;
                    _selectedGasto.Fecha = FechaPicker.Date;
                    _selectedGasto.EmpleadoId = empleado.Id;
                    _selectedGasto.DepartamentoId = departamento.Id;

                    context.Gastos.Update(_selectedGasto);
                }
                context.SaveChanges();
            }

            ResetForm();
            LoadData();
        }
        else
        {
            DisplayAlert("Advertencia", "Por favor, complete todos los campos.", "OK");
        }
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Gasto gasto)
        {
            _selectedGasto = gasto;
            DescripcionEntry.Text = gasto.Descripcion;
            MontoEntry.Text = gasto.Monto.ToString();
            FechaPicker.Date = gasto.Fecha;
            EmpleadoPicker.SelectedItem = EmpleadoPicker.ItemsSource.Cast<Empleado>()
                .FirstOrDefault(e => e.Id == gasto.EmpleadoId);
            DepartamentoPicker.SelectedItem = DepartamentoPicker.ItemsSource.Cast<Departamento>()
                .FirstOrDefault(d => d.Id == gasto.DepartamentoId);
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Gasto gasto)
        {
            using (var context = new AppDbContext())
            {
                context.Gastos.Remove(gasto);
                context.SaveChanges();
            }
            LoadData();
        }
    }

    private void OnEmpleadoClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EmpleadoPage()); // Navega a la vista de Empleados
    }

    private void ResetForm()
    {
        DescripcionEntry.Text = string.Empty;
        MontoEntry.Text = string.Empty;
        FechaPicker.Date = DateTime.Now;
        EmpleadoPicker.SelectedItem = null;
        DepartamentoPicker.SelectedItem = null;
        _selectedGasto = null;
    }

    private async void OnDepartamentoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DepartamentoPage());
    }
    private async void OnFiltrarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FiltradoFechasPage());
    }
}

