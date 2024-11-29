const uri = 'api/Usuarios';

let todos = [];



// Utilizar el Método GET de HTTP 

function getUsuario() {

    fetch(uri)

        .then(response => {

            if (!response.ok) {

                throw new Error(`HTTP error! Status: ${response.status}`);

            }

            return response.json();

        })

        .then(data => validarIngreso(data))

        .catch(error => console.error("No se logró cargar datos", error));

}



function validarIngreso(data) {

    const Codigo = document.getElementById("Codigo").value.trim();

    const Contrasena = document.getElementById("Contrasena").value.trim();

    const selectElement = document.getElementById("Roles");

    const Rol = selectElement.value.trim();



    // Validar que todos los campos tengan información 

    if (!Codigo || !Contrasena || !Rol) {

        alert('Todos los campos son obligatorios.');

        return;

    }



    // Buscar al usuario en los datos obtenidos 

    const usuarioEncontrado = data.find(item =>

        item.Codigo === Codigo &&

        item.Contrasena === Contrasena &&

        item.Rol === Rol

    );



    if (usuarioEncontrado) {

        alert('Usuario y Contraseña Correctos');

        // Redireccionar según el rol 

        switch (Rol) {

            case 'Administrador':

                window.location.href = '/ABD/Opciones';

                break;

            case 'Usuario Final':

                window.location.href = '/UsuarioFinal/Consultas';

                break;

            default:

                alert('Rol no reconocido.');

        }

    } else {

        alert('Usuario o contraseña incorrectos.');

    }

} 