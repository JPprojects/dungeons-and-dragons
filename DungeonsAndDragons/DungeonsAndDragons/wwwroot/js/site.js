function changePreview(selected) {
    var source = selected.options[selected.selectedIndex].text
    $("#preview").attr("src", "/images/species/" + source + ".png");
};