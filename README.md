# ProyectoJuego3D
proyecto para practicar en unity 3D


Se agregaron los archivos ".gitignore" y ".gitattributes"

Gitignore

Para evitar subir archivos inecesarios al repositorio 

Gitattributes

Para en caso de que metan algun archivo o texto se estandarise

## Unity: 
* Texto con interacción 
  - Cambiar: colliders correspondiente
  - Hacer que aparezca un mensaje al mirar algo interactuable
  - Mensajes e informacion correspondiente, ejemplo: impresora que marca?
* Mas modelos 3D en especial las cosas pequeñas
* Google cardboard
* optimizar borrar assets
* Personaje profesor y su mensaje

### Para ponerle el mensaje interactivo a un objeto:
- Arriba del inspector cambiar layer a interactable.
- se agrega como componente el script "Interactable Object VR".
- Agregarle texto personalizado para el objeto.
- En panel info prefab arrastrar el prefab del panel 3d dentro de assets.
- offset posicion para ubicar bien el mensaje.
- Tiene que tener boxcollider o la collider que corresponda.
