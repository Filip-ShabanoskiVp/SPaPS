$(document).ready(function () {

    $('.dropdown-select').select2({
        allowClear: true,
        theme: 'bootstrap-5'
    });

    var roleInput = document.querySelector("#Role");
    var NoOfEmployee = document.querySelector(".NoOfEmployees");
    var DateEstablishment = document.querySelector(".DateEstablishment");
    var ActivityIds = document.querySelector(".ActivityIds");

    roleInput?.addEventListener("change", () => {

        if (roleInput.value == "Изведувач") {
            NoOfEmployee.classList.remove("d-none");
            DateEstablishment.classList.remove("d-none");
            ActivityIds.classList.remove("d-none");
        } else {
            NoOfEmployee.classList.add("d-none");
            DateEstablishment.classList.add("d-none");
            ActivityIds.classList.add("d-none");

            document.querySelector("#NoOfEmployees").value = null;
            document.querySelector("#DateEstablishment").value = null;
            document.querySelector("#ActivityIds").value = null;
        }
    })
   
});