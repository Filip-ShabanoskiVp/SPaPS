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

    let serviceInput = document.querySelector("#ServiceId");

    if (serviceInput) {

        let BuildingType = document.querySelector(".BuildingTypeId");
        let BuildingSize = document.querySelector(".BuildingSize");
        let Color = document.querySelector(".Color");
        let NoOfWindows = document.querySelector(".NoOfWindows");
        let NoOfDoors = document.querySelector(".NoOfDoors");

        serviceInput.addEventListener("change", () => {

            BuildingType.classList.add("d-none");
            BuildingSize.classList.add("d-none");
            Color.classList.add("d-none");
            NoOfWindows.classList.add("d-none");
            NoOfDoors.classList.add("d-none");
            Color.classList.add("d-none");

            let serviceId = serviceInput.value;

            if (serviceId == null) {
                return;
            }

            if (serviceId == 1) {
                BuildingType.classList.remove("d-none");
                NoOfWindows.classList.remove("d-none");
                NoOfDoors.classList.remove("d-none");
            }

            else if (serviceId == 2) {
                BuildingType.classList.remove("d-none");
                BuildingSize.classList.remove("d-none");
                Color.classList.remove("d-none");
            }

            else if (serviceId == 3) {
                BuildingType.classList.remove("d-none");
                BuildingSize.classList.remove("d-none");
            }

        });
    }
});