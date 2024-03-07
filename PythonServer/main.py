import asyncio
import websockets
import torch
from transformers import AutoModelForImageClassification, AutoFeatureExtractor
from PIL import Image
import io
import datetime
import os

model_name = "akahana/asl-vit"
cache_dir = "./model_cache"

received_images_dir = "./received_images"
if not os.path.exists(received_images_dir):
    os.makedirs(received_images_dir)

model = AutoModelForImageClassification.from_pretrained(model_name, cache_dir=cache_dir)
feature_extractor = AutoFeatureExtractor.from_pretrained(model_name, cache_dir=cache_dir)

model.eval()

# Create an array starting from zero and match the number to the corresponding letter
letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z']

async def predict(websocket, path):
    async for message in websocket:
        image_stream = io.BytesIO(message)
        image = Image.open(image_stream).convert("RGB")

        timestamp = datetime.datetime.now().strftime("%Y%m%d%H%M%S")
        original_image_save_path = f"{received_images_dir}/{timestamp}_original.png"
        image.save(original_image_save_path)

        resize_size = 224
        resized_image = image.resize((resize_size, resize_size))

        resized_image_save_path = f"{received_images_dir}/{timestamp}_resized.png"
        resized_image.save(resized_image_save_path)

        inputs = feature_extractor(images=resized_image, return_tensors="pt")

        with torch.no_grad():
            outputs = model(**inputs)
            logits = outputs.logits
            probabilities = torch.nn.functional.softmax(logits, dim=-1)
            top_prob, top_catid = torch.max(probabilities, dim=1)

        predicted_letter = letters[top_catid.item()]
        response = f"Letter: {predicted_letter}"
        await websocket.send(response)

start_server = websockets.serve(predict, "localhost", 8765)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()