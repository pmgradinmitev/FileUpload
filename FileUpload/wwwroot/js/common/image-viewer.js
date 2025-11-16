function imagePreview() {
    var img = document.getElementById("thumbnail-image");
    var input = document.getElementById("image-input");
    input.onchange = evt => {
        const [file] = input.files;
        if (file) {
            img.src = URL.createObjectURL(file);
        }
    };
}
$(document).ready(function () {
    imagePreview();
});