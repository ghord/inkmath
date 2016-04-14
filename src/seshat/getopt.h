#pragma once

extern int     opterr,             
optind,            
optopt,            
optreset;            
extern char    *optarg;            



                                /*
                                * getopt --
                                *      Parse argc/argv argument vector.
                                */
int
getopt(int nargc, char * const nargv[], const char *ostr);