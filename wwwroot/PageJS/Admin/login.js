function errorLimit() {
    Swal.fire({

        //title: '<span style="font-size: 20px;font-style: normal">'@TempData["Message"].ToString()'</span>',

        title: '<span style="font-size: 20px;font-style: normal">Account is locked, Kindly contact Administrator</span>',

        //title: '<span style="font-size: 20px;font-style: normal">Maximum Attempt Reached. Enter Valid UserName and Password</span>',

        showCancelButton: false,

        confirmButtonText: `Ok`,

        heightAuto: false

    });
}
function errorInvalid() {
    Swal.fire({

        //title: '<span style="font-size: 20px;font-style: normal">'@TempData["Message"].ToString()'</span>',

        title: '<span style="font-size: 20px;font-style: normal">UserName or Password is incorrect!</span>',

        showCancelButton: false,

        confirmButtonText: `Ok`,

        heightAuto: false

    });
}

function errorLock() {
    Swal.fire({

        //title: '<span style="font-size: 20px;font-style: normal">'@TempData["Message"].ToString()'</span>',

        title: '<span style="font-size: 20px;font-style: normal">UserName is locked, Kindly contact Administrator</span>',

        showCancelButton: false,

        confirmButtonText: `Ok`,

        heightAuto: false

    });
}

function LoginChk() {
    Swal.fire({

        title: 'User Already Logged In! \n Do you want to force login?',

        showDenyButton: true,

        confirmButtonText: 'Yes',

        denyButtonText: `Cancel`,

    }).then((result) => {

        /* Read more about isConfirmed, isDenied below */

        if (result.isConfirmed) {

            window.location.href = '/v2/Admin/Login/ForceLogin';

        } else if (result.isDenied) {

        }

    })
}

function nullValues() {
    Swal.fire({

        //title: '<span style="font-size: 20px;font-style: normal">'@TempData["Message"].ToString()'</span>',

        title: '<span style="font-size: 20px;font-style: normal">Credentials cannot be empty!</span>',

        showCancelButton: false,

        confirmButtonText: `Ok`,

        heightAuto: false

    });
}
