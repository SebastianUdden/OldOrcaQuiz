﻿$('#jstree_allQ').jstree({
    'core': {
    'data' : [
       'Simple root node',
       {
         'text' : 'Root node 2',
         'state' : {
           'opened' : true,
           'selected' : true
         },
         'children' : [
           { 'text' : 'Child 1' },
           'Child 2'
         ]
      }
    ]
} });