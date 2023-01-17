from  bbwebservice.webserver import *
import json
import os

@register(route="/", type = MIME_TYPE.HTML)
def loader():
    return load_file("/Creator.html")

@register(route="/script.js", type = MIME_TYPE.JAVA_SCRIPT)
def loader():
    return load_file("/script.js")

@register(route='/texture', type= MIME_TYPE.PNG)
def image(args):
    print('test')
    return load_bin_file('/'+args[STORE_VARS.QUERY_STRING]['path'])

@post_handler(route='/endpoint',type=MIME_TYPE.TEXT)
def endpoint(args):
    try:
        jo = json.loads(args['payload'])
        new_jo = {"Chunks":[]}
        chunk = {"x":0,"y":0,"biome":0,"Data":[]}
        print('mats=',jo['materials'])
        for row in jo['materials']:
            r = {'row':row}
            chunk['Data'].append(r)
        new_jo['Chunks'].append(chunk)
        print(os.getcwd()+'/test1.json')
        with open(os.getcwd()+'/test1.json','w') as f:
            json.dump(new_jo,f)
    except Exception as e:
        print(e)

    return '200 OK'

start()
