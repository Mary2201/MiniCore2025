using MiniCore.Models;
using System.Collections.ObjectModel;

namespace MiniCore.Views;

public partial class DepartamentoPage : ContentPage
{
    public ObservableCollection<Departamento> Departamentos { get; set; } = new ObservableCollection<Departamento>();
    private Departamento _selectedDepartamento;

    public DepartamentoPage()
    {
        InitializeComponent();
        BindingContext = this; // Enlazar los datos al contexto de la vista
        LoadDepartamentos();   // Cargar los departamentos al iniciar
    }

    private void LoadDepartamentos()
    {
        try
        {
            Departamentos.Clear(); // Limpiar la lista actual

            // Conexión y carga de departamentos desde la base de datos
            using (var context = new AppDbContext())
            {
                var departamentos = context.Departamentos.ToList();
                foreach (var departamento in departamentos)
                {
                    Departamentos.Add(departamento); // Agregar departamentos a la lista
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Ocurrió un error al cargar los departamentos: {ex.Message}", "OK");
        }
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NombreDepartamentoEntry?.Text))
        {
            using (var context = new AppDbContext())
            {
                if (_selectedDepartamento == null)
                {
                    // Crear un nuevo departamento
                    var nuevoDepartamento = new Departamento { Nombre = NombreDepartamentoEntry.Text };
                    context.Departamentos.Add(nuevoDepartamento);
                }
                else
                {
                    // Actualizar departamento existente
                    _selectedDepartamento.Nombre = NombreDepartamentoEntry.Text;
                    context.Departamentos.Update(_selectedDepartamento);
                }
                context.SaveChanges();
            }

            // Limpiar el formulario
            NombreDepartamentoEntry.Text = string.Empty;
            _selectedDepartamento = null;

            // Recargar la lista de departamentos
            LoadDepartamentos();
        }
        else
        {
            DisplayAlert("Advertencia", "El nombre del departamento no puede estar vacío.", "OK");
        }
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Departamento departamento)
        {
            _selectedDepartamento = departamento;
            NombreDepartamentoEntry.Text = departamento.Nombre;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Departamento departamento)
        {
            using (var context = new AppDbContext())
            {
                context.Departamentos.Remove(departamento);
                context.SaveChanges();
            }
            LoadDepartamentos();
        }
    }

    private async void OnEmpleadoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmpleadoPage());
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