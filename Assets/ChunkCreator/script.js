
function sendData(d) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5000/endpoint");
    xhr.setRequestHeader("Content-Type", "application/json");

    let data = `{"materials":${JSON.stringify(d)}}`

    xhr.send(data);

}

function removeAllChildren(element) {
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}

function setBackgroundImage(svg, rect, imagePath) {
    removeAllChildren(svg)
    var img = new Image();
    img.src = imagePath;
    img.onload = function () {
        var defs = document.createElementNS("http://www.w3.org/2000/svg", "defs");
        var pattern = document.createElementNS("http://www.w3.org/2000/svg", "pattern");
        pattern.setAttribute("id", imagePath);
        pattern.setAttribute("patternUnits", "userSpaceOnUse");
        pattern.setAttribute("width", "100%");
        pattern.setAttribute("height", "100%");

        var image = document.createElementNS("http://www.w3.org/2000/svg", "image");
        image.setAttributeNS("http://www.w3.org/1999/xlink", "href", imagePath);
        image.setAttribute("x", 0);
        image.setAttribute("y", 0);
        image.setAttribute("width", "100%");
        image.setAttribute("height", "100%");
        pattern.appendChild(image);
        defs.appendChild(pattern);
        svg.appendChild(defs);
        rect.style.fill = `url(#${imagePath})`;
        svg.appendChild(rect)
    }
}



class TileSVG {
    constructor(id, width, numTilesPerRow, selector) {
        this.id = id;
        this.width = width;
        this.numTilesPerRow = numTilesPerRow;
        this.tiles = [];
        this.materials = Array(numTilesPerRow);
        for (var i = 0; i < numTilesPerRow; i++) {
            this.materials[i] = Array(numTilesPerRow).fill(0);
        }
        this.selector = selector
        this.svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    }

    generate() {
        const tileSize = this.width / this.numTilesPerRow;
        this.svg.setAttributeNS(null, "width", this.width);
        this.svg.setAttributeNS(null, "height", this.width);
        const container = document.getElementById(this.id);
        container.innerHTML = "";
        container.appendChild(this.svg);

        for (let i = 0; i < this.numTilesPerRow; i++) {
            for (let j = 0; j < this.numTilesPerRow; j++) {
                const rect = document.createElementNS("http://www.w3.org/2000/svg", "rect");
                rect.setAttributeNS(null, "x", 0);
                rect.setAttributeNS(null, "y", 0);
                rect.setAttributeNS(null, "width", tileSize);
                rect.setAttributeNS(null, "height", tileSize);
                rect.setAttributeNS(null, "fill", "white");
                rect.setAttributeNS(null, "stroke", "black");
                rect.setAttributeNS(null, "stroke-width", "1");
                const tileSvg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
                tileSvg.setAttributeNS(null, "x", j * tileSize);
                tileSvg.setAttributeNS(null, "y", i * tileSize);
                tileSvg.setAttributeNS(null, "width", tileSize);
                tileSvg.setAttributeNS(null, "height", tileSize);
                tileSvg.appendChild(rect);
                this.svg.appendChild(tileSvg);
                this.tiles.push(tileSvg);
                if (this.selector !== undefined) {
                    rect.addEventListener('mousedown', () => this.mouseDownHandler.bind(this)(rect, tileSvg, this.numTilesPerRow-i,j))
                }
            }
        }
    }

    mouseDownHandler(rect, svg, y, x) {
        let current_val = this.selector.getCurrentValue()
        setBackgroundImage(svg, rect, current_val[2])
        this.materials[y][x] = current_val[0]
    }

    export() {
        return this.materials
    }
}

class ColorPicker {
    constructor(id, width) {
        this.id = id;
        this.width = width;
        this.color = { r: 0, g: 0, b: 0 };
    }

    generate() {
        const container = document.getElementById(this.id);
        container.innerHTML = "";


        // Create color display rect
        const colorRect = document.createElement("div");
        colorRect.style.width = this.width + "px";
        colorRect.style.height = this.width + "px";
        colorRect.style.backgroundColor = "rgb(" + this.color.r + "," + this.color.g + "," + this.color.b + ")";
        colorRect.style.display = "block";
        container.appendChild(colorRect);

        // Create RGB sliders and color indication rectangles
        const sliderContainer = document.createElement("div");
        ["r", "g", "b"].forEach(channel => {
            const slider = document.createElement("input");
            slider.type = "range";
            slider.min = "0";
            slider.max = "255";
            slider.value = this.color[channel];
            slider.addEventListener("input", e => {
                this.color[channel] = e.target.value;
                colorRect.style.backgroundColor = "rgb(" + this.color.r + "," + this.color.g + "," + this.color.b + ")";
                colorIndicator.style.backgroundColor = "rgb(" + (channel === "r" ? this.color.r : 0) + "," + (channel === "g" ? this.color.g : 0) + "," + (channel === "b" ? this.color.b : 0) + ")";
            });
            sliderContainer.appendChild(slider);

            // Create color indication rectangle
            const colorIndicator = document.createElement("div");
            colorIndicator.style.width = "20px";
            colorIndicator.style.height = "20px";
            colorIndicator.style.backgroundColor = "rgb(" + (channel === "r" ? this.color.r : 0) + "," + (channel === "g" ? this.color.g : 0) + "," + (channel === "b" ? this.color.b : 0) + ")";
            sliderContainer.appendChild(colorIndicator);
        });
        container.appendChild(sliderContainer);
    }
}

class Dropdown {
    constructor(id, width, height) {
        this.id = id;
        this.width = width;
        this.height = height;
        this.cells = cells;
        this.dropdown = null;
        this.selectedImg = null;
    }

    generate() {
        this.dropdown = document.createElement("select");
        this.dropdown.classList.add("dropdown");
        this.dropdown.style.width = this.width + "px";
        this.dropdown.style.height = this.height + "px";
        this.dropdown.addEventListener("change", this.handleChange.bind(this));

        for (let i = 0; i < this.cells.length; i++) {
            const option = document.createElement("option");
            option.value = this.cells[i][0];
            option.innerHTML = this.cells[i][1];
            this.dropdown.appendChild(option);
        }
        const container = document.getElementById(this.id);
        container.appendChild(this.dropdown);

        this.selectedImg = document.createElement("img");
        this.selectedImg.style.marginLeft = "10px";
        container.appendChild(this.selectedImg);
    }

    handleChange() {
        this.selectedImg.src = this.getCurrentValue()[2]
    }

    getCurrentValue() {
        const selectedValue = this.dropdown.options[this.dropdown.selectedIndex].value;
        for (let i = 0; i < this.cells.length; i++) {
            if (this.cells[i][0] == selectedValue) {
                return this.cells[i]
            }
        }
    }
}          