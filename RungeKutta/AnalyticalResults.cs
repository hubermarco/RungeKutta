
using System;

namespace RungeKutta
{
    public class AnalyticalResults
    {
        // ****************************************
        // Impulse response
        // ****************************************
        // y'' + a*y' + b*y = 0
        // lamda^2 + a*lamda + b = 0
        // lamda1/2 = -a/2 +- (a^2-4b)^0.5/2
        // ((a^2-4b) < 0) => complex value in exponent of exponential function
        // ((D^2-w0^2) < 0) 
        // wr... Resonance angular frequency
        // wr = |D^2-w0^2|^0.5
        // y_impuls = C1*exp(lamda*t)+C2*exp(-lamda*t)
        // y_impuls = C1*exp((-a/2 + (a^2-4b)^(0.5)/2*j)*t) + C2*exp((-a/2 - (a^2-4b)^(0.5)/2*j)*t)
        // a = 0.5, b = 1
        // wr=0.968 1/s -> wr=2*pi/T -> Tr = 6.083s
        // D=0.25
        // 
        // Determine constants with help of starting conditions
        // y(t=0)=y0, y(t=0)'=v0
        // y(t=0)=y0=C1 + C2
        // y'(t=0)=v0=lamda1*C1+lamda2*C2
        // C1=(v0-y0*lamda2)/(lamda1-lamda2)
        // C2=-(v0-y0*lamda2)/(lamda1-lamda2)
        // y(t)=1/2*exp(-D*t)*(B*exp(j*wr*t)+B.*exp(-j*wr*t))
        // B=y0+-(j*v0+y0*D)/wr
        // wr=(w0^2-D^2)^0.5
        //
        // y0=0, v0=1
        // B= +-j/wr
        // sin(x)=(exp(j*x)-exp(-j*x))/(2*j)
        // -j=1/j
        // y(t)=1/2*exp(-D*t)*(j/wr*exp(j*wr*t)-j/wr*exp(-j*wr*t))
        // y(t)=(exp(-D*t)*(1/wr*exp(j*wr*t)-1/wr*exp(-j*wr*t)))/(2*j)
        // y(t)=(exp(-D*t)*(1/wr*sin(wr*t))
        // y(t)=(exp(-0.25*t)*(1.0327955589886*sin(0.96824583655185*t))
        public static double Y_impulse(double time) => 1.0327955589886 * Math.Exp(-time / 4) * Math.Sin(0.96824583655185 * time);

        // ***************************************************************
        // Step response calculated with integration of impulse response
        // ***************************************************************
        // ystep = int(exp(-0.25*t)*(1.0328*sin(0.9682*t),t)
        // ystep = -(0.778891^t*cos(0.9682*t)-0.258223*0.778891^t*sin(0.9682*t) + C
        // ystep(t=0)=0 => C=1
        // ystep = 1-(0.778891^t*cos(0.9682*t)-0.258223*0.778891^t*sin(0.9682*t)
        public static double Y_step(double time) => 1 - Math.Pow(0.77880078307141, time) * Math.Cos(0.96824583655185 * time) -
            0.258198888974715 * Math.Pow(0.77880078307141, time) * Math.Sin(0.96824583655185 * time);
    }
}
