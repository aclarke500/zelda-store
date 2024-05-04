from flask import Flask, request, jsonify
import numpy as np
from flask_cors import CORS, cross_origin
from sentence_transformers import SentenceTransformer
print('Loading model...')
model = SentenceTransformer('paraphrase-MiniLM-L6-v2')
print('Model loaded successfully!')

# Initialize the Flask application
app = Flask(__name__)
CORS(app)
# # Define a simple route
# @cross_origin(origins='*')  # This allows all origins
# @app.route('/')
# def home():
#     return "Hello, welcome to my Flask app!"

# # Define another route that accepts GET and POST requests
# @app.route('/greet', methods=['GET', 'POST'])
# def greet():
#     if request.method == 'POST':
#         # name = request.form.get('name', 'World')
#         return jsonify(message=f"Hello,!")
#     return jsonify("Hello, World!")

@app.route('/classify', methods=['POST', 'OPTIONS'])
@cross_origin(origins='*')  # This allows all origins
def classify():
    if request.method == 'POST':
        data = request.get_json()
        print(data)
        word = model.encode(data['word'])
        classes = ['Food', 'Weapons', 'Consumables', 'Miscellaneous']
        class_embeddings = model.encode(classes)
        distances = []
        for class_embedding in class_embeddings:
            distances.append(np.linalg.norm(word - class_embedding))

        min_distance = min(distances)
        min_index = distances.index(min_distance)
        data['class'] = classes[min_index]
        return jsonify(data)
    
@app.after_request
def add_cors_headers(response):
    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Access-Control-Allow-Methods'] = 'GET, POST, PUT, DELETE, OPTIONS'
    response.headers['Access-Control-Allow-Headers'] = 'Content-Type'
    return response

# Run the app
if __name__ == "__main__":
    app.run(debug=True)
