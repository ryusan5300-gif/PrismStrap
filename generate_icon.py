import math
from PIL import Image, ImageDraw

def create_prism_image(size):
    img = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    
    center = size / 2.0
    radius = size * 0.42
    
    glow_radius = size * 0.46
    for r in range(int(glow_radius), int(radius), -1):
        alpha = int(25 * (1.0 - (r - radius) / (glow_radius - radius)))
        draw.ellipse([center - r, center - r, center + r, center + r], fill=(99, 102, 241, alpha))

    top = (center, center - radius)
    bottom = (center, center + radius * 0.95)
    left = (center - radius * 0.85, center + radius * 0.25)
    right = (center + radius * 0.85, center + radius * 0.25)
    inner_center = (center, center + radius * 0.1)

    draw.polygon([top, inner_center, left], fill=(79, 70, 229, 255))
    draw.polygon([top, right, inner_center], fill=(168, 85, 247, 255))
    draw.polygon([left, inner_center, bottom], fill=(56, 189, 248, 255))
    draw.polygon([inner_center, right, bottom], fill=(236, 72, 153, 255))

    width = max(1, int(size * 0.02))
    draw.line([top, inner_center], fill=(255, 255, 255, 240), width=width)
    draw.line([inner_center, bottom], fill=(255, 255, 255, 200), width=width)
    draw.line([inner_center, left], fill=(255, 255, 255, 180), width=width)
    draw.line([inner_center, right], fill=(255, 255, 255, 180), width=width)
    draw.line([top, left, bottom, right, top], fill=(255, 255, 255, 220), width=width)

    return img

sizes = [256, 128, 64, 48, 32, 16]
images = [create_prism_image(s) for s in sizes]

images[0].save(
    "icon.ico",
    format="ICO",
    sizes=[(s, s) for s in sizes],
    append_images=images[1:]
)
print("Icon created successfully!")
