document.addEventListener('DOMContentLoaded', function () {
    handleProfileImageUpload(); 
    select();
})



function handleProfileImageUpload() {
    try {
        let fileUploader = document.querySelector('#fileUploader');
        let fileForm = document.querySelector('#profileImageForm');

        if (fileUploader && fileForm) {
            fileUploader.addEventListener('change', function () {
                if (this.files.length > 0) {
                    fileForm.submit();
                }
            });
        }
    } catch (error) {
        console.error('Error handling profile image upload:', error);
    }
}

function select() {
    try {
        let select = document.querySelector('.select')
        let selected = select.querySelector('.selected')
        let selectOptions = select.querySelector('.selectOptions')

        selected.addEventListener('click', function () {
            selectOptions.style.display = (selectOptions.style.display === 'block') ? 'none' : 'block'
        })

        let options = selectOptions.querySelectorAll('.option')
        option.forEach(function (option) {
            option.addEventListener('click', function () {
                selected.innerHTML = this.textContent
                selectOptions.style.display = 'none'

                let category = this.getAttribute('data-value')
            })
        })
    }
    catch {

    }
}