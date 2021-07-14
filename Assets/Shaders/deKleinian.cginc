uniform float Size;
uniform float DEoffset;
uniform int Iterations;
uniform float3 CSize;
uniform float3 C;
uniform float3 Offset;


float Thingy(float3 p, float e){
    p-=Offset;
    return (abs(length(p.xy)*p.z)-e) / sqrt(dot(p,p)+abs(e));
}

float de(float3 p, inout float3 orbitTrap){
    //Just scale=1 Julia box
    float DEfactor=1.;
    float3 arp=p+1.;
    for(int i=0;i<Iterations;i++){
        if (distance(arp, p) < 0.00001) break;
        arp=p;
        p=2.*clamp(p, -CSize, CSize)-p;
      
        float r2=dot(p,p);
        orbitTrap = min(orbitTrap, abs(float4(p,r2)));
        float k=max(Size/r2,1.);

        p*=k;DEfactor*=k;
      
        p+=C;
        orbitTrap = min(orbitTrap, abs(float4(p,dot(p,p))));
    }
    //Call basic shape and scale its DE
    //return abs(0.5*Thingy(p,TThickness)/DEfactor-DEoffset);
	
    //Alternative shape
    //return abs(0.5*RoundBox(p, vec3(1.,1.,1.), 1.0)/DEfactor-DEoffset);
    //Just a plane
    return abs(0.5*abs(p.z-Offset.z)/DEfactor-DEoffset);
}


float de(float3 p){
    //Just scale=1 Julia box
    float DEfactor=1.;
    float3 arp=p+1.;
    for(int i=0;i<Iterations;i++){
        if (distance(arp, p) < 0.00001) break;

        arp=p;
        p=2.*clamp(p, -CSize, CSize)-p;
      
        float r2=dot(p,p);
        float k=max(Size/r2,1.);

        p*=k;DEfactor*=k;
      
        p+=C;
    }
    //Call basic shape and scale its DE
    //return abs(0.5*Thingy(p,TThickness)/DEfactor-DEoffset);
	
    //Alternative shape
    //return abs(0.5*RoundBox(p, vec3(1.,1.,1.), 1.0)/DEfactor-DEoffset);
    //Just a plane
    return abs(0.5*abs(p.z-Offset.z)/DEfactor-DEoffset);
}