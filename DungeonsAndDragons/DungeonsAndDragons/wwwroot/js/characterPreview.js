var species = jQuery.parseJSON($("#json-species").text());

function changePreview(selected) {
    var speciesId = selected.options[selected.selectedIndex].value - 1; 
    $("#character-image").attr("src", species[speciesId].image_path);
    $("#character-hp").text( species[speciesId].base_hp);
    $("#character-attack").text( species[speciesId].base_attack);
};