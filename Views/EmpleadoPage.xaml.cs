using MiniCore.Models;
using System.Collections.ObjectModel;

namespace MiniCore.Views;

public partial class EmpleadoPage : ContentPage
{
    public ObservableCollection<Empleado> Empleados { get; set; } = new ObservableCollection<Empleado>();
    private Empleado _selectedEmpleado;

    public EmpleadoPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadEmpleados();
    }

    private void LoadEmpleados()
    {
        try
        {
            // Limpiar la lista antes de cargar nuevos datos
            Empleados.Clear();

            // Conexión y carga de empleados desde la base de datos
            using (var context = new AppDbContext())
            {
                var empleados = context.Empleados.ToList();
                foreach (var empleado in empleados)
                {
                    Empleados.Add(empleado);
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción (puedes registrar el error o mostrar un mensaje al usuario)
            DisplayAlert("Error", $"Ocurrió un error al cargar los empleados: {ex.Message}", "OK");
        }
    }


    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NombreEntry.Text))
        {
            using (var context = new AppDbContext())
            {
                if (_selectedEmpleado == null)
                {
                    // Crear un nuevo empleado
                    var nuevoEmpleado = new Empleado { Nombre = NombreEntry.Text };
                    context.Empleados.Add(nuevoEmpleado);
                }
                else
                {
                    // Actualizar empleado existente
                    _selectedEmpleado.Nombre = NombreEntry.Text;
                    context.Empleados.Update(_selectedEmpleado);
                }
                context.SaveChanges();
            }

            NombreEntry.Text = string.Empty;
            _selectedEmpleado = null;
            LoadEmpleados();
        }
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Empleado empleado)
        {
            _selectedEmpleado = empleado;
            NombreEntry.Text = empleado.Nombre;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Empleado empleado)
        {
            using (var context = new AppDbContext())
            {
                context.Empleados.Remove(empleado);
                context.SaveChanges();
            }
            LoadEmpleados();
        }
    }
    
    private async void OnDepartamentoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DepartamentoPage());
    }
    private async void OnGastosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GastoPage());
    }
    private async void OnFiltrarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FiltradoFechasPage());
    }
}